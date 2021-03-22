using Newtonsoft.Json;

namespace Binances.Helper.Models.UserStream
{
    public class UserStreamInfo
    {
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; }
    }
}
