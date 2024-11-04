using Cysharp.Threading.Tasks;
using Starxr.SDK.RestApiClient;
using Newtonsoft.Json.Linq;
using Starxr.SDK.AI.Interfaces;
using Starxr.SDK.AI.Data;

namespace Starxr.SDK.AI.Core
{
    internal class GoogleCloudServiceImpl : IGoogleCloudService
    {
        public async UniTask<string> CallSTT(
            string json,
            string sttAPIUrl,
            string googleCloudAPIKey)
        {
            string requestUri = $"{googleCloudAPIKey}";

            var headers = new RestApiRequestHeader()
                .SetContentType(RestApiClient.Defines.Header.ContentType.Application_Json);

            var options = new RestApiRequestOptions(requestUri, RestApiClient.Defines.RequestMethod.Post, true, sttAPIUrl)
                .SetBody(JObject.Parse(json))
                .SetHeader(headers);

            RestApiClient.RestApiClient client = new RestApiClient.RestApiClient(options);
            var transfer = await client.RequestToApiServer();
            var responseText = transfer.ConvertTo<GoogleCloudSTTResponseDto>();

            if (transfer.ResponseCode != RestApiClient.Defines.ResponseCode.Ok)
            {
                throw new RestApiResponseException(transfer.ResponseCode, responseText?.ErrorMessage ?? "Unknown error");
            }

            foreach (var result in responseText.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    return alternative.Transcript;
                }
            }

            return null;
        }
    }

}
