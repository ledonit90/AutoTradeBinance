﻿using Binances.Helper.Models.Enums;
using Remibit.Models.Binance;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Utils
{
    /// <summary>
    /// Utility class for common processes. 
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Gets a HMACSHA256 signature based on the API Secret.
        /// </summary>
        /// <param name="apiSecret">Api secret used to generate the signature.</param>
        /// <param name="message">Message to encode.</param>
        /// <returns></returns>
        public static string GenerateSignature(string apiSecret, string message)
        {
            var key = Encoding.UTF8.GetBytes(apiSecret);
            string stringHash;
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                stringHash = BitConverter.ToString(hash).Replace("-", "");
            }

            return stringHash;
        }

        /// <summary>
        /// Gets a timestamp in milliseconds.
        /// </summary>
        /// <returns>Timestamp in milliseconds.</returns>
        public static long GenerateTimeStamp(DateTime baseDateTime)
        {
            var dtOffset = new DateTimeOffset(baseDateTime);
            return dtOffset.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Gets an HttpMethod object based on a string.
        /// </summary>
        /// <param name="method">Name of the HttpMethod to create.</param>
        /// <returns>HttpMethod object.</returns>
        public static HttpMethod CreateHttpMethod(string method)
        {
            switch (method.ToUpper())
            {
                case "DELETE":
                    return HttpMethod.Delete;
                case "POST":
                    return HttpMethod.Post;
                case "PUT":
                    return HttpMethod.Put;
                case "GET":
                    return HttpMethod.Get;
                default:
                    throw new NotImplementedException();
            }
        }

        public static OrderStatus OrderStatusConverter(string value)
        {
            switch(value)
            {
                case "NEW":
                    return OrderStatus.NEW;
                case "PARTIALLY_FILLED":
                    return OrderStatus.PARTIALLY_FILLED;
                case "FILLED":
                    return OrderStatus.FILLED;
                case "CANCELED":
                    return OrderStatus.CANCELED;
                case "PENDING_CANCEL":
                    return OrderStatus.PENDING_CANCEL;
                case "REJECTED":
                    return OrderStatus.REJECTED;
                case "EXPIRED":
                    return OrderStatus.EXPIRED;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
