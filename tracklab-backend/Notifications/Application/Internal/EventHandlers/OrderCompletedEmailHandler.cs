using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Tracking.Domain.Events;
using Alumware.Tracklab.API.Notifications.Infrastructure.Email.Services;
using TrackLab.Notifications.Infrastructure.Email.Services;
using TrackLab.Notifications.Infrastructure.Email.Models;
using Microsoft.Extensions.Logging;
using TrackLab.IAM.Domain.Repositories;

namespace Alumware.Tracklab.API.Notifications.Application.Internal.EventHandlers;

/// <summary>
/// Handles OrderCompletedEvent to send email notifications to customer and logistics company
/// </summary>
public class OrderCompletedEmailHandler : IEventHandler<OrderCompletedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _templateService;
    private readonly ITenantRepository _tenantRepository;
    private readonly ILogger<OrderCompletedEmailHandler> _logger;

    public OrderCompletedEmailHandler(
        IEmailService emailService,
        IEmailTemplateService templateService,
        ITenantRepository tenantRepository,
        ILogger<OrderCompletedEmailHandler> logger)
    {
        _emailService = emailService;
        _templateService = templateService;
        _tenantRepository = tenantRepository;
        _logger = logger;
    }

    public async Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing OrderCompletedEvent for Order ID: {OrderId}", notification.OrderId);

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
            await SendCustomerCompletionEmail(notification, customerTenant, logisticsTenant.GetDisplayName());

            // Send email to logistics company
            await SendLogisticsCompletionEmail(notification, logisticsTenant, customerTenant.GetDisplayName());

            _logger.LogInformation("Completion emails sent successfully for Order ID: {OrderId}", notification.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling OrderCompletedEvent for Order ID: {OrderId}", notification.OrderId);
            throw;
        }
    }

    private async Task SendCustomerCompletionEmail(OrderCompletedEvent orderEvent, TrackLab.IAM.Domain.Model.Aggregates.Tenant customerTenant, string logisticsName)
    {
        var templateData = new OrderCompletedEmailData(
            customerTenant.GetDisplayName(),
            "customer",
            orderEvent.OrderId,
            customerTenant.GetDisplayName(),
            logisticsName,
            orderEvent.TotalContainers,
            orderEvent.TotalWeight,
            orderEvent.CompletedAt,
            orderEvent.DeliveryStats
        );

        var htmlBody = _templateService.GenerateOrderCompletedTemplate(templateData);
        var subject = $"¡Orden Completada! - #{orderEvent.OrderId}";
        
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
        
        _logger.LogInformation("Completion email sent to customer {CustomerName} for Order ID: {OrderId}", 
            customerTenant.GetDisplayName(), orderEvent.OrderId);
    }

    private async Task SendLogisticsCompletionEmail(OrderCompletedEvent orderEvent, TrackLab.IAM.Domain.Model.Aggregates.Tenant logisticsTenant, string customerName)
    {
        var templateData = new OrderCompletedEmailData(
            logisticsTenant.GetDisplayName(),
            "logistics",
            orderEvent.OrderId,
            customerName,
            logisticsTenant.GetDisplayName(),
            orderEvent.TotalContainers,
            orderEvent.TotalWeight,
            orderEvent.CompletedAt,
            orderEvent.DeliveryStats
        );

        var htmlBody = _templateService.GenerateOrderCompletedTemplate(templateData);
        var subject = $"Orden Completada Exitosamente - #{orderEvent.OrderId}";
        
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
        
        _logger.LogInformation("Completion email sent to logistics company {LogisticsName} for Order ID: {OrderId}", 
            logisticsTenant.GetDisplayName(), orderEvent.OrderId);
    }
} 