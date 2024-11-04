using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Data;

namespace Starxr.SDK.AI.Interfaces
{
    public interface ICustomVoiceService
    {
        UniTask<string> RegisterVoiceID(string name, string voiceData, string voiceScript);
        UniTask DeleteVoiceID(string voiceID);
        UniTask<VoiceDataDto> GetVoiceList();
    }
}
