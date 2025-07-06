using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TrackLab.Shared.Interfaces.REST.Resources;

public class UploadImageResource
{
    [Required]
    public IFormFile File { get; set; }
    
    public string? Folder { get; set; }
} 