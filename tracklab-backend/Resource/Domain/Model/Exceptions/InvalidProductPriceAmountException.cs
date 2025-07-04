namespace Alumware.Tracklab.API.Resource.Domain.Model.Exceptions;

[Serializable]
public class InvalidProductPriceAmountException :  Exception
{
    public  InvalidProductPriceAmountException(int priceAmount) : base("Invalid product price amount. Value: " + priceAmount) { }
}