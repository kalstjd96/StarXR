using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Core;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;

namespace Starxr.SDK.AI.Core
{
    public class CustomVoiceService : ICustomVoiceService
    {
        private readonly ICustomVoiceService _service;

        public CustomVoiceService(ICustomVoiceService serviceImpl = null)
        {
            _service = serviceImpl ?? new CustomVoiceServiceImpl();
        }

        public UniTask<string> RegisterVoiceID(string name, string voiceData, string voiceScript)
        {
            return _service.RegisterVoiceID(name, voiceData, voiceScript);
        }

        public UniTask DeleteVoiceID(string voiceID)
        {
            return _service.DeleteVoiceID(voiceID);
        }

        public UniTask<VoiceDataDto> GetVoiceList()
        {
            return _service.GetVoiceList();
        }
    }
}
