namespace Alumware.Tracklab.API.Order.Domain.Model.Queries;

public class GetOrderByIdQuery {
    public long Id { get; }
    public GetOrderByIdQuery(long id) => Id = id;
} 