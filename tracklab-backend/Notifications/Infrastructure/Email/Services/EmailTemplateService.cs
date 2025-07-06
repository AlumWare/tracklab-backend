using System.Text;

namespace Alumware.Tracklab.API.Notifications.Infrastructure.Email.Services;

/// <summary>
/// Service for generating email templates
/// </summary>
public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateOrderCreatedTemplate(OrderCreatedEmailData data)
    {
        var template = GetBaseTemplate();
        
        var title = data.RecipientType == "logistics" ? "¡Nueva Orden Asignada!" : "¡Orden Creada Exitosamente!";
        var greeting = data.RecipientType == "logistics" 
            ? $"Estimado equipo de <strong>{data.RecipientName}</strong>,"
            : $"Estimado/a <strong>{data.RecipientName}</strong>,";
        
        var message = data.RecipientType == "logistics"
            ? "Se ha creado una nueva orden que requiere sus servicios logísticos."
            : "Su orden ha sido creada exitosamente y ha sido asignada a nuestra empresa logística.";
        
        var actionButton = data.RecipientType == "logistics" 
            ? "Ver Orden en TrackLab"
            : "Seguir mi Orden";
        
        var content = $@"
            <h2 style='color: #2c3e50; margin-bottom: 20px;'>{title}</h2>
            
            <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                {greeting}
            </p>
            
            <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                {message}
            </p>
            
            <div style='background-color: #ecf0f1; padding: 20px; border-radius: 5px; margin: 20px 0;'>
                <h3 style='color: #2c3e50; margin-top: 0;'>Detalles de la Orden</h3>
                <p><strong>Número de Orden:</strong> #{data.OrderId}</p>
                <p><strong>Cliente:</strong> {data.CustomerName}</p>
                <p><strong>Empresa Logística:</strong> {data.LogisticsName}</p>
                <p><strong>Dirección de Envío:</strong> {data.ShippingAddress}</p>
                <p><strong>Total de Artículos:</strong> {data.TotalItems}</p>
                <p><strong>Valor Total:</strong> {data.TotalAmount:C} {data.Currency}</p>
                <p><strong>Fecha de Creación:</strong> {data.CreatedAt:dd/MM/yyyy HH:mm}</p>
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='#' style='background-color: #3498db; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                    {actionButton}
                </a>
            </div>";
        
        return template.Replace("{{CONTENT}}", content);
    }

    public string GenerateOrderStatusChangedTemplate(OrderStatusChangedEmailData data)
    {
        var template = GetBaseTemplate();
        
        var title = "Estado de su Orden Actualizado";
        var greeting = $"Estimado/a <strong>{data.RecipientName}</strong>,";
        
        var message = data.RecipientType == "logistics"
            ? $"El estado de la orden ha sido actualizado a <strong style='color: {data.StatusColor};'>{data.NewStatus}</strong>."
            : "Le informamos que el estado de su orden ha sido actualizado.";
        
        var statusMessage = GetStatusSpecificMessage(data.NewStatus);
        
        var content = $@"
            <h2 style='color: {data.StatusColor}; margin-bottom: 20px;'>{title}</h2>
            
            <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                {greeting}
            </p>
            
            <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                {message}
            </p>
            
            <div style='background-color: #ecf0f1; padding: 20px; border-radius: 5px; margin: 20px 0;'>
                <h3 style='color: #2c3e50; margin-top: 0;'>Detalles de la Actualización</h3>
                <p><strong>Número de Orden:</strong> #{data.OrderId}</p>
                <p><strong>Cliente:</strong> {data.CustomerName}</p>
                <p><strong>Empresa Logística:</strong> {data.LogisticsName}</p>
                <p><strong>Estado Anterior:</strong> {data.PreviousStatus}</p>
                <p><strong>Estado Actual:</strong> <span style='color: {data.StatusColor}; font-weight: bold;'>{data.NewStatus}</span></p>
                <p><strong>Fecha de Actualización:</strong> {data.UpdatedAt:dd/MM/yyyy HH:mm}</p>
                {(string.IsNullOrEmpty(data.Reason) ? "" : $"<p><strong>Motivo:</strong> {data.Reason}</p>")}
            </div>
            
            {statusMessage}
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='#' style='background-color: {data.StatusColor}; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                    Seguir Orden en TrackLab
                </a>
            </div>";
        
        return template.Replace("{{CONTENT}}", content);
    }

    public string GenerateOrderCompletedTemplate(OrderCompletedEmailData data)
    {
        var template = GetBaseTemplate();
        
        var title = data.RecipientType == "logistics" 
            ? "¡Orden Completada Exitosamente!" 
            : "¡Su Orden Ha Sido Completada!";
            
        var greeting = data.RecipientType == "logistics" 
            ? $"Estimado equipo de <strong>{data.RecipientName}</strong>,"
            : $"Estimado/a <strong>{data.RecipientName}</strong>,";
        
        var message = data.RecipientType == "logistics"
            ? "Felicitaciones por completar exitosamente la orden."
            : "Nos complace informarle que su orden ha sido completada exitosamente.";
        
        var actionButton = data.RecipientType == "logistics" 
            ? "Ver Resumen de Operación"
            : "Ver Detalles de la Orden";
        
        var content = $@"
            <h2 style='color: #27ae60; margin-bottom: 20px;'>{title}</h2>
            
            <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                {greeting}
            </p>
            
            <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                {message}
            </p>
            
            <div style='background-color: #d5f4e6; padding: 20px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #27ae60;'>
                <h3 style='color: #27ae60; margin-top: 0;'>Resumen de {(data.RecipientType == "logistics" ? "Operación" : "Entrega")}</h3>
                <p><strong>Número de Orden:</strong> #{data.OrderId}</p>
                <p><strong>Cliente:</strong> {data.CustomerName}</p>
                <p><strong>Empresa Logística:</strong> {data.LogisticsName}</p>
                <p><strong>Contenedores Entregados:</strong> {data.TotalContainers}</p>
                <p><strong>Peso Total:</strong> {data.TotalWeight:F2} kg</p>
                <p><strong>Fecha de Finalización:</strong> {data.CompletedAt:dd/MM/yyyy HH:mm}</p>
            </div>
            
            <div style='background-color: #ecf0f1; padding: 20px; border-radius: 5px; margin: 20px 0;'>
                <h3 style='color: #2c3e50; margin-top: 0;'>Estadísticas de Entrega</h3>
                <p>{data.DeliveryStats}</p>
            </div>
            
            <div style='text-align: center; margin: 30px 0;'>
                <a href='#' style='background-color: #27ae60; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                    {actionButton}
                </a>
            </div>";
        
        return template.Replace("{{CONTENT}}", content);
    }

    private string GetBaseTemplate()
    {
        return @"
        <html>
        <body style='font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f5f5f5;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                {{CONTENT}}
                
                <p style='color: #7f8c8d; font-size: 14px; border-top: 1px solid #ecf0f1; padding-top: 20px; margin-top: 30px;'>
                    Este es un mensaje automático de TrackLab. Por favor no responda a este correo.
                </p>
            </div>
        </body>
        </html>";
    }

    private string GetStatusSpecificMessage(string status)
    {
        return status switch
        {
            "En Proceso" => @"
                <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                    Su orden está siendo procesada por nuestro equipo logístico. Se asignarán los recursos necesarios para su entrega.
                </p>",
            "En Tránsito" => @"
                <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                    Su orden está en tránsito y en camino a su destino. Podrá seguir el progreso en tiempo real a través de nuestra plataforma.
                </p>",
            "Entregada" => @"
                <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                    ¡Su orden ha sido entregada exitosamente! Gracias por confiar en nuestros servicios.
                </p>",
            "Cancelada" => @"
                <p style='color: #34495e; font-size: 16px; line-height: 1.6;'>
                    Su orden ha sido cancelada. Si tiene alguna pregunta, por favor contacte con nuestro equipo de soporte.
                </p>",
            _ => ""
        };
    }
} 