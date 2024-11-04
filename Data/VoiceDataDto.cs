using Starxr.SDK.RestApiClient;
using System;
using System.Collections.Generic;

namespace Starxr.SDK.AI.Data
{
    [Serializable]
    public class VoiceDataDto : DataTransferObjectBase
    {
        public List<VoiceDto> voice_list;
    }

    [Serializable]
    public class VoiceDto
    {
        public string voice_id;
        public string name;
        public string voice_script;
    }
}
