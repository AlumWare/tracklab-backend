namespace Alumware.Tracklab.API.Resource.Domain.Model.Queries;

public class GetAllProductsQuery 
{
    public int? PageSize { get; }
    public int? PageNumber { get; }
    public string? Name { get; }
    public string? Category { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    
    public GetAllProductsQuery(int? pageSize = null, int? pageNumber = null, string? name = null, string? category = null, decimal? minPrice = null, decimal? maxPrice = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        Name = name;
        Category = category;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
    }
} 