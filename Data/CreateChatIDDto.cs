using Newtonsoft.Json;
using Starxr.SDK.AI.Core;
using Starxr.SDK.RestApiClient;
using UnityEngine.Networking;

namespace Starxr.SDK.AI.Data
{
    public class CreateChatIDDto : DataTransferObjectBase
    {
        [JsonProperty("chat_id")]
        public string ChatID { get; set; } = string.Empty;
    }

    public class RequestGptServerDto : DataTransferObjectBase
    {
        public string NpcID { get; set; }
        public string VoiceID { get; set; }
        public string ChatID { get; set; }
        public string ServerURL { get; set; }
        public string QueryText { get; set; }
        public bool Streaming { get; set; }
        public string Language { get; set; }
        public bool UseTTS { get; set; }
        public string QueryImage { get; set; }

        public RequestGptServerDto(
            string aICategoryNPCID,
            string aIVoiceID,
            string chatId,
            string queryText,
            bool streaming,
            string language,
            bool useTTS,
            string queryImage = "")
        {
            NpcID = aICategoryNPCID;
            VoiceID = aIVoiceID;
            ChatID = chatId;
            ServerURL = GPTSettings.Instance.DefaultServerURL;
            QueryText = UnityWebRequest.EscapeURL(queryText);
            Streaming = streaming;
            Language = language.ToString();
            UseTTS = useTTS;
            QueryImage = string.IsNullOrEmpty(queryImage) ? "" : queryImage;
        }
    }

    public class ResponseGptServerDto : DataTransferObjectBase
    {
        [JsonProperty("query")]
        public string Query { get; set; } = string.Empty;

        [JsonProperty("voice_data")]
        public string VoiceData { get; set; } = string.Empty;

        [JsonProperty("answer")]
        public string Answer { get; set; } = string.Empty;
    }

}
