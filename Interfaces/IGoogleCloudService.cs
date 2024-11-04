using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Data;
using System;
using UnityEngine;
namespace Starxr.SDK.AI.Interfaces
{
    public interface IGoogleCloudService
    {
        UniTask<string> CallSTT(string json, string sttAPIUrl, string googleCloudAPIKey);
    }
}
