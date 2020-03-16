using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Remibit.Models.SupportObj
{
    public interface IPriceHub
    {
        Task SendMessageAsync(string message);
    }
}
