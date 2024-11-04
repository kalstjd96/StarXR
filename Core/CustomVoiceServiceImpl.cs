using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using Starxr.SDK.RestApiClient;
using UnityEngine.Networking;

namespace Starxr.SDK.AI.Core
{
    internal class CustomVoiceServiceImpl : ICustomVoiceService
    {
        internal class RequestCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        /// <summary>
        /// 목소리 등록
        /// </summary>
        /// <param name="name"></param>
        /// <param name="voiceData"></param>
        /// <param name="voiceScript"></param>
        /// <returns></returns>
        public async UniTask<string> RegisterVoiceID(string name, string voiceData, string voiceScript)
        {
            string url = "/voice";

            var voiceDto = new
            {
                name = name,
                voice_data = voiceData,
                voice_script = voiceScript
            };

            JObject jsonObject = JObject.FromObject(voiceDto);

            var headers = new RestApiRequestHeader()
              .SetContentType(RestApiClient.Defines.Header.ContentType.Application_Json);

            var options = new RestApiRequestOptions(url, RestApiClient.Defines.RequestMethod.Post, true,
                GPTSettings.Instance.DefaultServerURL)
               .SetBody(jsonObject)
                .SetHeader(headers);

            RestApiClient.RestApiClient client = new(options);
            var certificateHandler = new RequestCertificate();
            var transfer = await client.RequestToApiServer(null, certificateHandler);
            var result = transfer.ConvertTo<VoiceIDDto>();

            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(
                    transfer.ResponseCode,
                    result?.ErrorMessage ?? "Unknown error");
            }

            return result.VoiceID;
        }


        /// <summary>
        /// 등록된 목소리 정보 삭제
        /// </summary>
        /// <param name="voiceID"></param>
        /// <returns></returns>
        public async UniTask DeleteVoiceID(string voiceID)
        {
            string url = $"/voice/{voiceID}";

            var options = new RestApiRequestOptions(url, RestApiClient.Defines.RequestMethod.Delete, true,
                GPTSettings.Instance.DefaultServerURL);

            RestApiClient.RestApiClient client = new RestApiClient.RestApiClient(options);
            var certificateHandler = new RequestCertificate();
            var transfer = await client.RequestToApiServer(null, certificateHandler);

            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(transfer.ResponseCode);
            }
        }

        /// <summary>
        /// 목소리 조회
        /// </summary>
        /// <param name="name"></param>
        /// <param name="voiceData"></param>
        /// <param name="voiceScript"></param>
        /// <returns></returns>
        public async UniTask<VoiceDataDto> GetVoiceList()
        {
            string url = "/voice";

            var headers = new RestApiRequestHeader()
                .SetContentType(RestApiClient.Defines.Header.ContentType.Application_Json);

            var options = new RestApiRequestOptions(url, RestApiClient.Defines.RequestMethod.Get, true, 
                GPTSettings.Instance.DefaultServerURL)
                .SetHeader(headers);

            RestApiClient.RestApiClient client = new RestApiClient.RestApiClient(options);
            var certificateHandler = new RequestCertificate();
            var transfer = await client.RequestToApiServer(null, certificateHandler);

            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(
                    transfer.ResponseCode,
                    "Failed to retrieve voice list.");
            }

            var result = transfer.ConvertTo<VoiceDataDto>();
            return result;
        }
    }
}
