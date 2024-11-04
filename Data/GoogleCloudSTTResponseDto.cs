using Newtonsoft.Json;
using Starxr.SDK.RestApiClient;

namespace Starxr.SDK.AI.Data
{
    public class GoogleCloudSTTResponseDto : DataTransferObjectBase
    {
        [JsonProperty("audioContent")]
        public string AudioContent { get; set; }

        [JsonProperty("results")]
        public ResultDto[] Results { get; set; }
    }

    public class ResultDto : DataTransferObjectBase
    {
        [JsonProperty("alternatives")]
        public AlternativeDto[] Alternatives { get; set; }
    }

    public class AlternativeDto : DataTransferObjectBase
    {
        [JsonProperty("transcript")]
        public string Transcript { get; set; }
    }
}
