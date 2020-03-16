using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Exceptions
{
    public class WebsocketBadInputException : WebsocketException
    {
        public WebsocketBadInputException()
        {
        }

        public WebsocketBadInputException(string message) : base(message)
        {
        }

        public WebsocketBadInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
