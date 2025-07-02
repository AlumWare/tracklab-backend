namespace Alumware.Tracklab.API.Order.Interfaces.ACL;

public interface IOrdersContextFacade
{
    Task<long> CreateOrder(
        long customerId,
        long logisticsId,
        string shippingAddress);
    
    
}