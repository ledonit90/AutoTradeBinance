﻿using Remibit.Utility.Common;
using Remibit.Utility.RabitMQ;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Binances.Helper
{
    public class BinanceStream
    {
        private string listenKey = "";
        private string symbol = "";
        private ClientWebSocket _client;
        public string hostUri = "wss://stream.binance.com:9443/stream?streams=btcusdt@trade";
        public BinanceStream()
        {
            // chang can gi ca.
        }

        public async Task getDataStream(string hostUri)
        {
            Publisher test = new Publisher(AppConstConfig.PRICE_QEXCHANGE, AppConstConfig.PRICE_QNAME);
            using (ClientWebSocket ws = new ClientWebSocket())
            {
                Uri serverUri = new Uri(this.hostUri);
                await ws.ConnectAsync(serverUri, CancellationToken.None);
                while (ws.State == WebSocketState.Open)
                {
                    //    Thread.Sleep(100);
                    //    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes("sga"));
                    //    await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                    ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[10000]);
                    WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                    var messageString = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                    test.sendAMessage(messageString);
                }
            }
        }
    }
}
