using Newtonsoft.Json;

namespace SYTUCA.DataModels
{
    public class scoreInformation
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Score")]
        public string Score { get; set; }

    }
}
