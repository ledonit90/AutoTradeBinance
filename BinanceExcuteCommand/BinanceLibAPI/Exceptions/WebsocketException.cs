using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibAPI.Exceptions
{
    public class WebsocketException : Exception
    {
        public WebsocketException()
        {
        }

        public WebsocketException(string message)
            : base(message)
        {
        }

        public WebsocketException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
