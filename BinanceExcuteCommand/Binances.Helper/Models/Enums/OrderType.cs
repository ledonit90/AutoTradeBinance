using ServiceStack.DataAnnotations;

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
        [Description("NEW")]
        NEW,
        [Description("PARTIALLY_FILLED")]
        PARTIALLY_FILLED,
        [Description("FILLED")]
        FILLED,
        [Description("CANCELED")]
        CANCELED,
        [Description("PENDING_CANCEL")]
        PENDING_CANCEL,
        [Description("REJECTED")]
        REJECTED,
        [Description("EXPIRED")]
        EXPIRED
    }
}
