namespace Alumware.Tracklab.API.Notifications.Infrastructure.Email.Services;

/// <summary>
/// Service for generating email templates
/// </summary>
public interface IEmailTemplateService
{
    /// <summary>
    /// Generates HTML content for order created email
    /// </summary>
    /// <param name="data">Email data</param>
    /// <returns>HTML content</returns>
    string GenerateOrderCreatedTemplate(OrderCreatedEmailData data);
    
    /// <summary>
    /// Generates HTML content for order status changed email
    /// </summary>
    /// <param name="data">Email data</param>
    /// <returns>HTML content</returns>
    string GenerateOrderStatusChangedTemplate(OrderStatusChangedEmailData data);
    
    /// <summary>
    /// Generates HTML content for order completed email
    /// </summary>
    /// <param name="data">Email data</param>
    /// <returns>HTML content</returns>
    string GenerateOrderCompletedTemplate(OrderCompletedEmailData data);
}

/// <summary>
/// Data for order created email template
/// </summary>
public record OrderCreatedEmailData(
    string RecipientName,
    string RecipientType, // "customer" or "logistics"
    long OrderId,
    string CustomerName,
    string LogisticsName,
    string ShippingAddress,
    decimal TotalAmount,
    string Currency,
    int TotalItems,
    DateTime CreatedAt
);

/// <summary>
/// Data for order status changed email template
/// </summary>
public record OrderStatusChangedEmailData(
    string RecipientName,
    string RecipientType, // "customer" or "logistics"
    long OrderId,
    string CustomerName,
    string LogisticsName,
    string PreviousStatus,
    string NewStatus,
    string StatusColor,
    string? Reason,
    DateTime UpdatedAt
);

/// <summary>
/// Data for order completed email template
/// </summary>
public record OrderCompletedEmailData(
    string RecipientName,
    string RecipientType, // "customer" or "logistics"
    long OrderId,
    string CustomerName,
    string LogisticsName,
    int TotalContainers,
    decimal TotalWeight,
    DateTime CompletedAt,
    string DeliveryStats
); 