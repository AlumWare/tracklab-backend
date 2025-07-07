using TrackLab.Shared.Domain.Events;
using Alumware.Tracklab.API.Order.Domain.Events;
using Alumware.Tracklab.API.Notifications.Infrastructure.Email.Services;
using TrackLab.Notifications.Infrastructure.Email.Services;
using TrackLab.Notifications.Infrastructure.Email.Models;
using Microsoft.Extensions.Logging;
using TrackLab.IAM.Domain.Repositories;

namespace Alumware.Tracklab.API.Notifications.Application.Internal.EventHandlers;

/// <summary>
/// Handles OrderCreatedEvent to send email notifications to logistics company
/// </summary>
public class OrderCreatedEmailHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _templateService;
    private readonly ITenantRepository _tenantRepository;
    private readonly ILogger<OrderCreatedEmailHandler> _logger;

    public OrderCreatedEmailHandler(
        IEmailService emailService,
        IEmailTemplateService templateService,
        ITenantRepository tenantRepository,
        ILogger<OrderCreatedEmailHandler> logger)
    {
        _emailService = emailService;
        _templateService = templateService;
        _tenantRepository = tenantRepository;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing OrderCreatedEvent for Order ID: {OrderId}", notification.OrderId);

            // Get logistics company information
            var logisticsTenant = await _tenantRepository.FindByIdAsync(notification.LogisticsId);
            if (logisticsTenant == null)
            {
                _logger.LogWarning("Logistics tenant not found for ID: {LogisticsId}", notification.LogisticsId);
                return;
            }

            // Get customer information
            var customerTenant = await _tenantRepository.FindByIdAsync(notification.CustomerId);
            if (customerTenant == null)
            {
                _logger.LogWarning("Customer tenant not found for ID: {CustomerId}", notification.CustomerId);
                return;
            }

            // Send email to logistics company
            await SendLogisticsNotificationEmail(notification, customerTenant.GetDisplayName(), logisticsTenant);

            // Send confirmation email to customer
            await SendCustomerConfirmationEmail(notification, customerTenant, logisticsTenant.GetDisplayName());

            _logger.LogInformation("OrderCreated emails sent successfully for Order ID: {OrderId}", notification.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling OrderCreatedEvent for Order ID: {OrderId}", notification.OrderId);
            throw;
        }
    }

    private async Task SendLogisticsNotificationEmail(OrderCreatedEvent orderEvent, string customerName, TrackLab.IAM.Domain.Model.Aggregates.Tenant logisticsTenant)
    {
        var templateData = new OrderCreatedEmailData(
            logisticsTenant.GetDisplayName(),
            "logistics",
            orderEvent.OrderId,
            customerName,
            logisticsTenant.GetDisplayName(),
            orderEvent.ShippingAddress,
            orderEvent.TotalAmount,
            orderEvent.Currency,
            orderEvent.TotalItems,
            orderEvent.OccurredOn
        );

        var htmlBody = _templateService.GenerateOrderCreatedTemplate(templateData);
        var subject = $"Nueva Orden Asignada - #{orderEvent.OrderId}";
        
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
        
        _logger.LogInformation("Email sent to logistics company {LogisticsName} for Order ID: {OrderId}", 
            logisticsTenant.GetDisplayName(), orderEvent.OrderId);
    }

    private async Task SendCustomerConfirmationEmail(OrderCreatedEvent orderEvent, TrackLab.IAM.Domain.Model.Aggregates.Tenant customerTenant, string logisticsName)
    {
        var templateData = new OrderCreatedEmailData(
            customerTenant.GetDisplayName(),
            "customer",
            orderEvent.OrderId,
            customerTenant.GetDisplayName(),
            logisticsName,
            orderEvent.ShippingAddress,
            orderEvent.TotalAmount,
            orderEvent.Currency,
            orderEvent.TotalItems,
            orderEvent.OccurredOn
        );

        var htmlBody = _templateService.GenerateOrderCreatedTemplate(templateData);
        var subject = $"Orden Creada Exitosamente - #{orderEvent.OrderId}";
        
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
        
        _logger.LogInformation("Confirmation email sent to customer {CustomerName} for Order ID: {OrderId}", 
            customerTenant.GetDisplayName(), orderEvent.OrderId);
    }
} 