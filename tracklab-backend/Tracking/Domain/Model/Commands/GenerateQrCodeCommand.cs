namespace Alumware.Tracklab.API.Tracking.Domain.Model.Commands;

public record GenerateQrCodeCommand(
    long ContainerId,
    string TrackingUrl,
    int Size = 256
); 