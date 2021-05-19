using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper.Models.WebSocket
{
    public class StreamMessage<T> where T : class
    {
        public string stream { get; set; }
        public T data { get; set; }
    }
}
