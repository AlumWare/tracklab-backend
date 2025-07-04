using Alumware.Tracklab.API.Order.Domain.Model.ValueObjects;

namespace Alumware.Tracklab.API.Order.Domain.Model.Queries;

public class GetAllOrdersQuery 
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public OrderStatus? Status { get; }
    public long? CustomerId { get; }
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }
    
    public GetAllOrdersQuery(int? pageSize = null, int? pageNumber = null, OrderStatus? status = null, long? customerId = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Status = status;
        CustomerId = customerId;
        FromDate = fromDate;
        ToDate = toDate;
    }
} 