using Starxr.SDK.RestApiClient;
using System;
using UnityEngine;
namespace Starxr.SDK.AI.Data
{
    [Serializable]
    public class VoiceResponseDto : DataTransferObjectBase
    {
        public string voice_id;

        public override string ToString()
        {
            return $"Voice ID: {voice_id}";
        }
    }

}
