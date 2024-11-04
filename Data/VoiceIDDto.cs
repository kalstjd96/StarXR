using Newtonsoft.Json;
using Starxr.SDK.RestApiClient;
using System;
namespace Starxr.SDK.AI.Data
{
    [Serializable]
    public class VoiceIDDto : DataTransferObjectBase
    {
        [JsonProperty("voice_id")]
        public string VoiceID { get; set; } = string.Empty;
    }
}
