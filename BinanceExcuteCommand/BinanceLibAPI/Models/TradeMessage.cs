namespace BinanceLibAPI.Models
{
    public class StreamMessage
    {
        public string stream { get; set; }
        public TradeMessage data { get; set; }
    }

    public class TradeMessage
    {
        public string e { get; set; }   // Event type
        public long E { get; set; }     // Event time
        public string s { get; set; }   // Symbol
        public int t { get; set; }      // Trade ID
        public double p { get; set; }   // Price
        public string q { get; set; }   // Quantity
        public int b { get; set; }      // Buyer order ID
        public int a { get; set; }      // Seller order ID
        public long T { get; set; }     // Trade time
        public bool m { get; set; }     // Is the buyer the market maker? => true -> phần trên màu đỏ, người đặt lệnh mua khớp lệnh, => false -> màu xanh bên dưới -> người mua đặt lệnh
        public bool M { get; set; }     // Ignore
    }

}
