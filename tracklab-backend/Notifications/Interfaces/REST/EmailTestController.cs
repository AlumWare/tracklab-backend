using Microsoft.AspNetCore.Mvc;
using TrackLab.Notifications.Infrastructure.Email.Models;
using TrackLab.Notifications.Infrastructure.Email.Services;

namespace TrackLab.Notifications.Interfaces.REST;

[ApiController]
[Route("api/test/[controller]")]
public class EmailTestController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailTestController> _logger;
    private readonly string _templatesPath;

    public EmailTestController(IEmailService emailService, ILogger<EmailTestController> logger)
    {
        _emailService = emailService;
        _logger = logger;
        _templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notifications", "Infrastructure", "Email", "Templates");
    }

    private async Task<string> GetTemplateContent(string templateName)
    {
        var path = Path.Combine(_templatesPath, $"{templateName}.html");
        if (!System.IO.File.Exists(path))
        {
            throw new FileNotFoundException($"Template {templateName} no encontrado");
        }
        return await System.IO.File.ReadAllTextAsync(path);
    }

    private string ReplaceTemplateVariables(string template, Dictionary<string, string> variables)
    {
        foreach (var variable in variables)
        {
            template = template.Replace($"{{{variable.Key}}}", variable.Value);
        }
        return template;
    }

    /// <summary>
    /// Envía un correo de prueba con la plantilla de estado de orden
    /// </summary>
    [HttpPost("send/status")]
    public async Task<IActionResult> SendOrderStatusEmail([FromBody] OrderEmailRequest request)
    {
        try
        {
            var template = await GetTemplateContent("order-status");
            
            var variables = new Dictionary<string, string>
            {
                { "CustomerName", request.Name },
                { "OrderId", request.OrderId },
                { "OrderStatus", request.OrderStatus },
                { "TrackingUrl", $"https://tracklab.com/orders/{request.OrderId}" }
            };

            var html = ReplaceTemplateVariables(template, variables);

            var message = new EmailMessage
            {
                Subject = $"TrackLab - Actualización de Orden #{request.OrderId}",
                HtmlBody = html,
                To = new List<string> { request.To }
            };

            var success = await _emailService.SendEmailAsync(message);

            if (success)
            {
                return Ok(new { message = "Correo de estado enviado exitosamente" });
            }

            return BadRequest(new { error = "No se pudo enviar el correo de estado" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar correo de estado");
            return StatusCode(500, new { error = "Error interno al enviar el correo" });
        }
    }

    /// <summary>
    /// Envía un correo de prueba con la plantilla de orden completada
    /// </summary>
    [HttpPost("send/completed")]
    public async Task<IActionResult> SendOrderCompletedEmail([FromBody] OrderCompletedRequest request)
    {
        try
        {
            var template = await GetTemplateContent("order-completed");
            
            var variables = new Dictionary<string, string>
            {
                { "CustomerName", request.Name },
                { "OrderId", request.OrderId },
                { "DeliveryDate", request.DeliveryDate.ToString("dd/MM/yyyy HH:mm") },
                { "DeliveryLocation", request.DeliveryLocation },
                { "FeedbackUrl", $"https://tracklab.com/orders/{request.OrderId}/feedback" }
            };

            var html = ReplaceTemplateVariables(template, variables);

            var message = new EmailMessage
            {
                Subject = $"TrackLab - ¡Tu orden #{request.OrderId} ha sido entregada!",
                HtmlBody = html,
                To = new List<string> { request.To }
            };

            var success = await _emailService.SendEmailAsync(message);

            if (success)
            {
                return Ok(new { message = "Correo de orden completada enviado exitosamente" });
            }

            return BadRequest(new { error = "No se pudo enviar el correo de orden completada" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar correo de orden completada");
            return StatusCode(500, new { error = "Error interno al enviar el correo" });
        }
    }
}

public class OrderEmailRequest
{
    public string To { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
}

public class OrderCompletedRequest
{
    public string To { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public DateTime DeliveryDate { get; set; }
    public string DeliveryLocation { get; set; } = string.Empty;
} 