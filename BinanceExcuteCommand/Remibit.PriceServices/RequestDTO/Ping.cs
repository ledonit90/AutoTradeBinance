using ServiceStack;

namespace Remibit.PriceServices.RequestDTO
{
    [Route("/ping")]
    public class Ping : IReturn<object>
    {
        public bool IncludeCmitSha { get; set; }
    }
}
