using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Remibit.Models.Binance
{
    public partial class ExchangeInfoQuickTypes
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }

        [JsonProperty("rateLimits")]
        public RateLimit[] RateLimits { get; set; }

        [JsonProperty("exchangeFilters")]
        public object[] ExchangeFilters { get; set; }

        [JsonProperty("symbols")]
        public Symbol[] Symbols { get; set; }
    }

    public partial class RateLimit
    {
        [JsonProperty("rateLimitType")]
        public string RateLimitType { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("intervalNum")]
        public long IntervalNum { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }
    }

    public partial class Symbol
    {
        [JsonProperty("symbol")]
        public string SymbolSymbol { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; }

        [JsonProperty("baseAssetPrecision")]
        public long BaseAssetPrecision { get; set; }

        [JsonProperty("quoteAsset")]
        public QuoteAsset QuoteAsset { get; set; }

        [JsonProperty("quotePrecision")]
        public long QuotePrecision { get; set; }

        [JsonProperty("orderTypes")]
        public OrderType[] OrderTypes { get; set; }

        [JsonProperty("icebergAllowed")]
        public bool IcebergAllowed { get; set; }

        [JsonProperty("isSpotTradingAllowed")]
        public bool IsSpotTradingAllowed { get; set; }

        [JsonProperty("isMarginTradingAllowed")]
        public bool IsMarginTradingAllowed { get; set; }

        [JsonProperty("filters")]
        public Filter[] Filters { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("filterType")]
        public FilterType FilterType { get; set; }

        [JsonProperty("minPrice", NullValueHandling = NullValueHandling.Ignore)]
        public string MinPrice { get; set; }

        [JsonProperty("maxPrice", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxPrice { get; set; }

        [JsonProperty("tickSize", NullValueHandling = NullValueHandling.Ignore)]
        public string TickSize { get; set; }

        [JsonProperty("multiplierUp", NullValueHandling = NullValueHandling.Ignore)]
        public string MultiplierUp { get; set; }

        [JsonProperty("multiplierDown", NullValueHandling = NullValueHandling.Ignore)]
        public string MultiplierDown { get; set; }

        [JsonProperty("avgPriceMins", NullValueHandling = NullValueHandling.Ignore)]
        public long? AvgPriceMins { get; set; }

        [JsonProperty("minQty", NullValueHandling = NullValueHandling.Ignore)]
        public string MinQty { get; set; }

        [JsonProperty("maxQty", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxQty { get; set; }

        [JsonProperty("stepSize", NullValueHandling = NullValueHandling.Ignore)]
        public string StepSize { get; set; }

        [JsonProperty("minNotional", NullValueHandling = NullValueHandling.Ignore)]
        public string MinNotional { get; set; }

        [JsonProperty("applyToMarket", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ApplyToMarket { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? Limit { get; set; }

        [JsonProperty("maxNumAlgoOrders", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxNumAlgoOrders { get; set; }
    }

    public enum FilterType { IcebergParts, LotSize, MarketLotSize, MaxNumAlgoOrders, MinNotional, PercentPrice, PriceFilter };

    public enum OrderType { Limit, LimitMaker, Market, StopLossLimit, TakeProfitLimit };

    public enum QuoteAsset { Bnb, Btc, Eth, Pax, Tusd, Usdc, Usds, Usdt, Xrp };

    public enum Status { Break, Trading };

    public partial class ExchangeInfoQuickTypes
    {
        public static ExchangeInfoQuickTypes FromJson(string json) => JsonConvert.DeserializeObject<ExchangeInfoQuickTypes>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ExchangeInfoQuickTypes self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                FilterTypeConverter.Singleton,
                OrderTypeConverter.Singleton,
                QuoteAssetConverter.Singleton,
                StatusConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class FilterTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FilterType) || t == typeof(FilterType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "ICEBERG_PARTS":
                    return FilterType.IcebergParts;
                case "LOT_SIZE":
                    return FilterType.LotSize;
                case "MARKET_LOT_SIZE":
                    return FilterType.MarketLotSize;
                case "MAX_NUM_ALGO_ORDERS":
                    return FilterType.MaxNumAlgoOrders;
                case "MIN_NOTIONAL":
                    return FilterType.MinNotional;
                case "PERCENT_PRICE":
                    return FilterType.PercentPrice;
                case "PRICE_FILTER":
                    return FilterType.PriceFilter;
            }
            throw new Exception("Cannot unmarshal type FilterType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FilterType)untypedValue;
            switch (value)
            {
                case FilterType.IcebergParts:
                    serializer.Serialize(writer, "ICEBERG_PARTS");
                    return;
                case FilterType.LotSize:
                    serializer.Serialize(writer, "LOT_SIZE");
                    return;
                case FilterType.MarketLotSize:
                    serializer.Serialize(writer, "MARKET_LOT_SIZE");
                    return;
                case FilterType.MaxNumAlgoOrders:
                    serializer.Serialize(writer, "MAX_NUM_ALGO_ORDERS");
                    return;
                case FilterType.MinNotional:
                    serializer.Serialize(writer, "MIN_NOTIONAL");
                    return;
                case FilterType.PercentPrice:
                    serializer.Serialize(writer, "PERCENT_PRICE");
                    return;
                case FilterType.PriceFilter:
                    serializer.Serialize(writer, "PRICE_FILTER");
                    return;
            }
            throw new Exception("Cannot marshal type FilterType");
        }

        public static readonly FilterTypeConverter Singleton = new FilterTypeConverter();
    }

    internal class OrderTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OrderType) || t == typeof(OrderType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "LIMIT":
                    return OrderType.Limit;
                case "LIMIT_MAKER":
                    return OrderType.LimitMaker;
                case "MARKET":
                    return OrderType.Market;
                case "STOP_LOSS_LIMIT":
                    return OrderType.StopLossLimit;
                case "TAKE_PROFIT_LIMIT":
                    return OrderType.TakeProfitLimit;
            }
            throw new Exception("Cannot unmarshal type OrderType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OrderType)untypedValue;
            switch (value)
            {
                case OrderType.Limit:
                    serializer.Serialize(writer, "LIMIT");
                    return;
                case OrderType.LimitMaker:
                    serializer.Serialize(writer, "LIMIT_MAKER");
                    return;
                case OrderType.Market:
                    serializer.Serialize(writer, "MARKET");
                    return;
                case OrderType.StopLossLimit:
                    serializer.Serialize(writer, "STOP_LOSS_LIMIT");
                    return;
                case OrderType.TakeProfitLimit:
                    serializer.Serialize(writer, "TAKE_PROFIT_LIMIT");
                    return;
            }
            throw new Exception("Cannot marshal type OrderType");
        }

        public static readonly OrderTypeConverter Singleton = new OrderTypeConverter();
    }

    internal class QuoteAssetConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(QuoteAsset) || t == typeof(QuoteAsset?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "BNB":
                    return QuoteAsset.Bnb;
                case "BTC":
                    return QuoteAsset.Btc;
                case "ETH":
                    return QuoteAsset.Eth;
                case "PAX":
                    return QuoteAsset.Pax;
                case "TUSD":
                    return QuoteAsset.Tusd;
                case "USDC":
                    return QuoteAsset.Usdc;
                case "USDS":
                    return QuoteAsset.Usds;
                case "USDT":
                    return QuoteAsset.Usdt;
                case "XRP":
                    return QuoteAsset.Xrp;
            }
            throw new Exception("Cannot unmarshal type QuoteAsset");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (QuoteAsset)untypedValue;
            switch (value)
            {
                case QuoteAsset.Bnb:
                    serializer.Serialize(writer, "BNB");
                    return;
                case QuoteAsset.Btc:
                    serializer.Serialize(writer, "BTC");
                    return;
                case QuoteAsset.Eth:
                    serializer.Serialize(writer, "ETH");
                    return;
                case QuoteAsset.Pax:
                    serializer.Serialize(writer, "PAX");
                    return;
                case QuoteAsset.Tusd:
                    serializer.Serialize(writer, "TUSD");
                    return;
                case QuoteAsset.Usdc:
                    serializer.Serialize(writer, "USDC");
                    return;
                case QuoteAsset.Usds:
                    serializer.Serialize(writer, "USDS");
                    return;
                case QuoteAsset.Usdt:
                    serializer.Serialize(writer, "USDT");
                    return;
                case QuoteAsset.Xrp:
                    serializer.Serialize(writer, "XRP");
                    return;
            }
            throw new Exception("Cannot marshal type QuoteAsset");
        }

        public static readonly QuoteAssetConverter Singleton = new QuoteAssetConverter();
    }

    internal class StatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "BREAK":
                    return Status.Break;
                case "TRADING":
                    return Status.Trading;
            }
            throw new Exception("Cannot unmarshal type Status");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Status)untypedValue;
            switch (value)
            {
                case Status.Break:
                    serializer.Serialize(writer, "BREAK");
                    return;
                case Status.Trading:
                    serializer.Serialize(writer, "TRADING");
                    return;
            }
            throw new Exception("Cannot marshal type Status");
        }

        public static readonly StatusConverter Singleton = new StatusConverter();
    }
}
