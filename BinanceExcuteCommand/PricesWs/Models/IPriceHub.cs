using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pricesws.Models
{
    public interface IPriceHub
    {
        Task SendMessageAsync(string message);
    }
}
