namespace Binances.Helper.Models.Enums
{
    /// <summary>
    /// Different types of an order.
    /// </summary>
    public enum OrderType
    {
        LIMIT,
        MARKET
    }

    public enum OrderStatus
    {
        NEW,
        PARTIALLY_FILLED,
        FILLED,
        CANCELED,
        PENDING_CANCEL,
        REJECTED,
        EXPIRED
    }
}
