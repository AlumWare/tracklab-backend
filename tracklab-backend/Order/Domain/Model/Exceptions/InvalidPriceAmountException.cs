namespace Alumware.Tracklab.API.Order.Domain.Model.Exceptions;

[Serializable]
public class InvalidPriceAmountException : Exception
{
    public InvalidPriceAmountException(int priceAmount) : base("Invalid price amount. Price: " + priceAmount) {}
}