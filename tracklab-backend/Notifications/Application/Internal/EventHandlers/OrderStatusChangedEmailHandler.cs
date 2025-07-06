using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Order.Domain.Events;
using Alumware.Tracklab.API.Notifications.Infrastructure.Email.Services;
using TrackLab.Notifications.Infrastructure.Email.Services;
using TrackLab.Notifications.Infrastructure.Email.Models;
using Microsoft.Extensions.Logging;
using TrackLab.IAM.Domain.Repositories;
using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Notifications.Application.Internal.EventHandlers;

/// <summary>
/// Handles OrderStatusChangedEvent to send email notifications about status changes
/// </summary>
public class OrderStatusChangedEmailHandler : IEventHandler<OrderStatusChangedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _templateService;
    private readonly ITenantRepository _tenantRepository;
    private readonly ILogger<OrderStatusChangedEmailHandler> _logger;

    public OrderStatusChangedEmailHandler(
        IEmailService emailService,
        IEmailTemplateService templateService,
        ITenantRepository tenantRepository,
        ILogger<OrderStatusChangedEmailHandler> logger)
    {
        _emailService = emailService;
        _templateService = templateService;
        _tenantRepository = tenantRepository;
        _logger = logger;
    }

    public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing OrderStatusChangedEvent for Order ID: {OrderId} - Status: {PreviousStatus} -> {NewStatus}", 
                notification.OrderId, notification.PreviousStatus, notification.NewStatus);

            // Only send emails for significant status changes
            if (!ShouldSendEmailForStatusChange(notification.PreviousStatus, notification.NewStatus))
            {
                _logger.LogDebug("Skipping email notification for status change: {PreviousStatus} -> {NewStatus}", 
                    notification.PreviousStatus, notification.NewStatus);
                return;
            }

            // Get customer and logistics information
            var customerTenant = await _tenantRepository.FindByIdAsync(notification.CustomerId);
            var logisticsTenant = await _tenantRepository.FindByIdAsync(notification.LogisticsId);

            if (customerTenant == null)
            {
                _logger.LogWarning("Customer tenant not found for ID: {CustomerId}", notification.CustomerId);
                return;
            }

            if (logisticsTenant == null)
            {
                _logger.LogWarning("Logistics tenant not found for ID: {LogisticsId}", notification.LogisticsId);
                return;
            }

            // Send email to customer
            await SendCustomerStatusChangeEmail(notification, customerTenant, logisticsTenant.GetDisplayName());

            // Send email to logistics company for certain status changes
            if (ShouldNotifyLogisticsCompany(notification.NewStatus))
            {
                await SendLogisticsStatusChangeEmail(notification, logisticsTenant, customerTenant.GetDisplayName());
            }

            _logger.LogInformation("Status change emails sent successfully for Order ID: {OrderId}", notification.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling OrderStatusChangedEvent for Order ID: {OrderId}", notification.OrderId);
            throw;
        }
    }

    private bool ShouldSendEmailForStatusChange(OrderStatus previousStatus, OrderStatus newStatus)
    {
        // Send emails for these important status changes
        return newStatus switch
        {
            OrderStatus.InProcess => true,
            OrderStatus.Shipped => true,
            OrderStatus.Delivered => true,
            OrderStatus.Cancelled => true,
            _ => false
        };
    }

    private bool ShouldNotifyLogisticsCompany(OrderStatus newStatus)
    {
        // Notify logistics company for these statuses
        return newStatus switch
        {
            OrderStatus.Cancelled => true,
            OrderStatus.Delivered => true,
            _ => false
        };
    }

    private async Task SendCustomerStatusChangeEmail(OrderStatusChangedEvent statusEvent, TrackLab.IAM.Domain.Model.Aggregates.Tenant customerTenant, string logisticsName)
    {
        var templateData = new OrderStatusChangedEmailData(
            customerTenant.GetDisplayName(),
            "customer",
            statusEvent.OrderId,
            customerTenant.GetDisplayName(),
            logisticsName,
            GetStatusDisplayName(statusEvent.PreviousStatus),
            GetStatusDisplayName(statusEvent.NewStatus),
            GetStatusColor(statusEvent.NewStatus),
            statusEvent.Reason,
            statusEvent.OccurredOn
        );

        var htmlBody = _templateService.GenerateOrderStatusChangedTemplate(templateData);
        var subject = GetSubjectForStatusChange(statusEvent.NewStatus, statusEvent.OrderId);
        
        var customerEmail = customerTenant.GetEmail();
        if (string.IsNullOrEmpty(customerEmail))
        {
            _logger.LogWarning("No email found for customer tenant {CustomerId}", customerTenant.Id);
            return;
        }

        var emailMessage = new EmailMessage
        {
            To = new List<string> { customerEmail },
            Subject = subject,
            HtmlBody = htmlBody
        };

        await _emailService.SendEmailAsync(emailMessage);
    }

    private async Task SendLogisticsStatusChangeEmail(OrderStatusChangedEvent statusEvent, TrackLab.IAM.Domain.Model.Aggregates.Tenant logisticsTenant, string customerName)
    {
        var templateData = new OrderStatusChangedEmailData(
            logisticsTenant.GetDisplayName(),
            "logistics",
            statusEvent.OrderId,
            customerName,
            logisticsTenant.GetDisplayName(),
            GetStatusDisplayName(statusEvent.PreviousStatus),
            GetStatusDisplayName(statusEvent.NewStatus),
            GetStatusColor(statusEvent.NewStatus),
            statusEvent.Reason,
            statusEvent.OccurredOn
        );

        var htmlBody = _templateService.GenerateOrderStatusChangedTemplate(templateData);
        var subject = GetLogisticsSubjectForStatusChange(statusEvent.NewStatus, statusEvent.OrderId);
        
        var logisticsEmail = logisticsTenant.GetEmail();
        if (string.IsNullOrEmpty(logisticsEmail))
        {
            _logger.LogWarning("No email found for logistics tenant {LogisticsId}", logisticsTenant.Id);
            return;
        }

        var emailMessage = new EmailMessage
        {
            To = new List<string> { logisticsEmail },
            Subject = subject,
            HtmlBody = htmlBody
        };

        await _emailService.SendEmailAsync(emailMessage);
    }

    private string GetSubjectForStatusChange(OrderStatus newStatus, long orderId)
    {
        return newStatus switch
        {
            OrderStatus.InProcess => $"Su Orden #{orderId} está en Proceso",
            OrderStatus.Shipped => $"Su Orden #{orderId} está en Envío",
            OrderStatus.Delivered => $"Su Orden #{orderId} ha sido Entregada",
            OrderStatus.Cancelled => $"Su Orden #{orderId} ha sido Cancelada",
            _ => $"Actualización de Estado - Orden #{orderId}"
        };
    }

    private string GetLogisticsSubjectForStatusChange(OrderStatus newStatus, long orderId)
    {
        return newStatus switch
        {
            OrderStatus.Cancelled => $"Orden #{orderId} ha sido Cancelada",
            OrderStatus.Delivered => $"Orden #{orderId} Entregada Exitosamente",
            _ => $"Actualización de Estado - Orden #{orderId}"
        };
    }

    private string GetStatusDisplayName(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "Pendiente",
            OrderStatus.InProcess => "En Proceso",
            OrderStatus.Shipped => "En Envío",
            OrderStatus.Delivered => "Entregada",
            OrderStatus.Cancelled => "Cancelada",
            _ => status.ToString()
        };
    }

    private string GetStatusColor(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "#f39c12",
            OrderStatus.InProcess => "#3498db",
            OrderStatus.Shipped => "#9b59b6",
            OrderStatus.Delivered => "#27ae60",
            OrderStatus.Cancelled => "#e74c3c",
            _ => "#95a5a6"
        };
    }
} 