using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Order.Interfaces.REST.Resources;

public record UpdateOrderStatusResource(
    string Status
); 