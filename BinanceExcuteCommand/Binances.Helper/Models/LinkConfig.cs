﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models
{
    public class LinkConfig
    {
        public static readonly string ExchangeInfo = "https://www.binance.com/api/v1/exchangeInfo";
        public static readonly string exChangeInfo = "https://www.binance.com/api/v1/exchangeInfo";
        public static readonly string serverTime = "https://www.binance.com/api/v1/time";
        public static readonly string DepositAddress = "https://www.binance.com//wapi/v3/depositAddress.html";

        public static readonly string Test_connectivity = "https://www.binance.com/api/api/v1/ping";
        // Parameters: NONE
        // -----------------------------Market Data endpoints----------------------------
        // Name	        Type	Mandatory   Description
        public static readonly string Order_book_depth = "https://www.binance.com/api/v1/depth";
        // symbol	    STRING	YES	
        // limit	    INT		NO
        public static readonly string Recent_trades_list = "https://www.binance.com/api/v1/trades";
        // symbol	    STRING	YES	
        // limit	    INT		NO
        public static readonly string Old_trade_lookup = "https://www.binance.com/api/v1/historicalTrades";
        // symbol       STRING  YES
        // limit        INT     NO          Default 500; max 1000.
        // fromId       LONG    NO          TradeId to fetch from.Default gets most recent trades.
        public static readonly string Aggregate_trades_list = "https://www.binance.com/api/v1/aggTrades";
        // symbol       STRING  YES
        // fromId       LONG    NO          ID to get aggregate trades from INCLUSIVE.
        // startTime    LONG    NO          Timestamp in ms to get aggregate trades from INCLUSIVE.
        // endTime      LONG    NO          Timestamp in ms to get aggregate trades until INCLUSIVE.
        // limit        INT     NO          Default 500; max 1000.
        public static readonly string Candlestick_data = "https://www.binance.com/api/v1/klines";
        // symbol       STRING  YES
        // interval     ENUM    YES
        // startTime    LONG    NO
        // endTime      LONG    NO
        // limit        INT     NO          Default 500; max 1000.
        public static readonly string Current_average_price = "https://www.binance.com/api/v3/avgPrice";
        // symbol	    STRING	YES	
        public static readonly string ticker_24hr_price_change_statistics = "https://www.binance.com/api/v1/ticker/24hr";
        // symbol       STRING  NO
        public static readonly string Symbol_price_ticker = "https://www.binance.com/api/v3/ticker/price";
        // symbol	    STRING	NO
        public static readonly string Symbol_order_book_ticker = "https://www.binance.com/api/v3/ticker/bookTicker";
        // symbol	    STRING	NO


        // -------------------------------------------- Account endpoints -----------------------------------------
        public static readonly string New_order = "https://api.binance.com/api/v3/order";
        // symbol       STRING  YES
        // side         ENUM    YES
        // type         ENUM    YES
        // timeInForce  ENUM    NO
        // quantity     DECIMAL YES
        // price        DECIMAL NO
        // newClientOrderId STRING  NO      A unique id for the order.Automatically generated if not sent.
        // stopPrice    DECIMAL NO          Used with STOP_LOSS, STOP_LOSS_LIMIT, TAKE_PROFIT, and TAKE_PROFIT_LIMIT orders.
        // icebergQty   DECIMAL NO          Used with LIMIT, STOP_LOSS_LIMIT, and TAKE_PROFIT_LIMIT to create an iceberg order.
        // newOrderRespType ENUM    NO Set the response JSON.ACK, RESULT, or FULL; MARKET and LIMIT order types default to FULL, all other orders default to ACK.
        // recvWindow   LONG     NO
        // timestamp    LONG    YES
        public static readonly string Query_order = "https://www.binance.com/api/v3/order";
        // symbol       STRING  YES
        // orderId      LONG    NO
        // origClientOrderId STRING  NO
        // recvWindow   LONG    NO
        // timestamp    LONG    YES
        public static readonly string Cancel_order = "https://www.binance.com/api/v3/order";
        // symbol       STRING  YES
        // orderId      LONG    NO
        // origClientOrderId STRING  NO
        // newClientOrderId    STRING NO  Used to uniquely identify this cancel.Automatically generated by default.
        // recvWindow   LONG    NO
        // timestamp    LONG    YES
        public static readonly string Current_open_orders = "https://www.binance.com/api/v3/openOrders";
        // symbol       STRING  NO
        // recvWindow   LONG    NO
        // timestamp    LONG    YES
        public static readonly string All_orders = "https://www.binance.com/api/v3/allOrders";
        // symbol       STRING  YES
        // orderId      LONG    NO
        // startTime    LONG    NO
        // endTime      LONG    NO
        // limit        INT     NO          Default 500; max 1000.
        // recvWindow   LONG    NO
        // timestamp    LONG    YES
        public static readonly string Account_information  = "https://www.binance.com/api/v3/account";
        // recvWindow   LONG    NO
        // timestamp    LONG    YES
        public static readonly string Account_trade_list = "https://www.binance.com/api/v3/myTrades";
        // symbol       STRING  YES
        // startTime    LONG    NO
        // endTime      LONG    NO
        // fromId       LONG    NO  T       radeId to fetch from.Default gets most recent trades.
        // limit        INT     NO          Default 500; max 1000.
        // recvWindow   LONG    NO
        // timestamp    LONG    YES

        //---------------------------- User data stream endpoints ---------------------------------------------------------
        public static readonly string Start_user_data_stream = "https://www.binance.com/api/v1/userDataStream";
        // POST 
        // Parameters: NONE
        public static readonly string Keepalive_user_data_stream = "https://www.binance.com/api/v1/userDataStream";
        //PUT
        // listenKey    STRING  YES
        public static readonly string Close_user_data_stream = "https://www.binance.com/api/v1/userDataStream";
        // DELETE 
        // listenKey    STRING  YES
    }
}