using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using Starxr.SDK.RestApiClient;
using UnityEngine.Networking;

namespace Starxr.SDK.AI.Core
{
    internal class GptCommunicationServiceImpl : IGptCommunicationService
    {
        internal class RequestCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        public async UniTask<string> CreateChatID()
        {
            string url = "/chat";
            var headers = new RestApiRequestHeader()
                .SetCustomHeader("X-API-KEY", GPTSettings.Instance.GPTAPIKey);

            var options = new RestApiRequestOptions(url, RestApiClient.Defines.RequestMethod.Post, true,
                GPTSettings.Instance.DefaultServerURL)
                .SetHeader(headers);

            RestApiClient.RestApiClient client = new RestApiClient.RestApiClient(options);

            var certificateHandler  = new RequestCertificate();
            var transfer = await client.RequestToApiServer(null, certificateHandler);
            var result = transfer.ConvertTo<CreateChatIDDto>();

            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(transfer.ResponseCode, result?.ErrorMessage ?? "Unknown error");
            }

            return result.ChatID;
        }

        public async UniTask DeleteChatID(string chatID)
        {
            string url = $"/chat/{chatID}";

            var options = new RestApiRequestOptions(url, RestApiClient.Defines.RequestMethod.Delete, true,
                GPTSettings.Instance.DefaultServerURL)
                .SetHeader(new RestApiRequestHeader()
                .SetCustomHeader("X-API-KEY", GPTSettings.Instance.GPTAPIKey)
                );

            RestApiClient.RestApiClient client = new RestApiClient.RestApiClient(options);
            var certificateHandler = new RequestCertificate();
            var transfer = await client.RequestToApiServer(null, certificateHandler); 
            
            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(transfer.ResponseCode);
            }
        }

        public async UniTask<ResponseGptServerDto> SendMessageToGptServer(RequestGptServerDto requestDto)
        {
            string serverURL = $"{requestDto.ServerURL}/chat/{requestDto.ChatID}";
            string url = "/ask";

            var headers = new RestApiRequestHeader()
                .SetCustomHeader("X-API-KEY", GPTSettings.Instance.GPTAPIKey)
                .SetCustomHeader("Content-Type", "application/json");

            var options = new RestApiRequestOptions(url, RestApiClient.Defines.RequestMethod.Post, true, serverURL)
                .SetHeader(headers)
                .SetBody(JObject.FromObject(new
                {
                    npc_id = requestDto.NpcID,
                    voice_id = requestDto.VoiceID,
                    query_text = requestDto.QueryText,
                    streaming = requestDto.Streaming,
                    language = requestDto.Language.ToString(),
                    use_TTS = requestDto.UseTTS,
                    query_image = string.IsNullOrEmpty(requestDto.QueryImage) ? "" : requestDto.QueryImage,
                    api_key = GPTSettings.Instance.GPTAPIKey
                }));

            RestApiClient.RestApiClient client = new RestApiClient.RestApiClient(options);
            var certificateHandler = new RequestCertificate();
            var transfer = await client.RequestToApiServer(null, certificateHandler);
            var result = transfer.ConvertTo<ResponseGptServerDto>();

            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(transfer.ResponseCode, result.ErrorMessage);
            }

            result.Answer = CleanResponse(result.Answer);
            return result;
        }

        private string CleanResponse(string response)
        {
            return response.Replace("[EOS]", "").Trim();
        }
    }
}
