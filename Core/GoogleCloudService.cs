using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using Starxr.SDK.AI.Interfaces;
using Starxr.SDK.AI.Data;

namespace Starxr.SDK.AI.Core
{
    public class GoogleCloudService : IGoogleCloudService
    {
        private IGoogleCloudService _service;

        public GoogleCloudService(IGoogleCloudService service = null)
        {
            _service = service ?? new GoogleCloudServiceImpl();
        }

        public UniTask<string> CallSTT(
            string json,
            string sttAPIUrl,
            string googleCloudAPIKey)
        {
            return _service.CallSTT(json, sttAPIUrl, googleCloudAPIKey);
        }
    }
}
