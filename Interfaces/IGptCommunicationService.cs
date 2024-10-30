/**
 * @author #AUTHOR#@naviworks.com
 * @brief
 * @version 0.1
 * @date 2024-10-16 09:58:22Z
 *
 * @copyright Copyright 2024 Naviworks, Co., LTD. All rights reserved.
 *
 */
using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Data;

namespace Starxr.SDK.AI.Interfaces
{
    public interface IGptCommunicationService
    {
        UniTask<string> CreateChatID();
        UniTask DeleteChatID(string chatID);
        UniTask<ResponseGptServerDto> SendMessageToGptServer(RequestGptServerDto requestDto);
    }
}
