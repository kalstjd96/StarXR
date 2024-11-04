using System;
using System.Collections.Generic;

namespace Starxr.SDK.AI.Data
{
    [Serializable]
    public class MicrophoneDevice
    {
        public string label;
        public string deviceId;
    }

    [Serializable]
    public class MicrophoneList
    {
        public List<MicrophoneDevice> devices;
    }
}
