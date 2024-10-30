/**
 * @author minsung.kim@naviworks.com
 * @brief
 * @version 0.1
 * @date 2024-10-15 08:51:09Z
 *
 * @copyright Copyright 2024 Naviworks, Co., LTD. All rights reserved.
 *
 */
using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using System;
using UnityEngine;

namespace Starxr.SDK.AI.Core
{
    public class GptCommunicationService : IGptCommunicationService
    {
        private readonly IGptCommunicationService _service;

        /// <summary>
        /// 생성자 주입을 통한 서비스 초기화
        /// </summary>
        /// <param name="aiData">AI 데이터</param>
        /// <param name="service">구현체 주입 (테스트 시 Mock 주입 가능)</param>
        public GptCommunicationService(IGptCommunicationService service = null)
        {
            _service = service ?? new GptCommunicationServiceImpl();
        }

        public async UniTask<string> CreateChatID()
        {
            try
            {
                var result = await _service.CreateChatID();
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Chat ID 생성 중 오류 발생: {ex.Message}");
                return null;

            }
        }

        public async UniTask DeleteChatID(string chatID)
        {
            if (string.IsNullOrEmpty(chatID))
            {
                Debug.LogWarning("유효하지 않은 Chat ID입니다.");
                return;
            }
            await _service.DeleteChatID(chatID);
        }


        public async UniTask<ResponseGptServerDto> SendMessageToGptServer(RequestGptServerDto requestDto)
        {
            return await _service.SendMessageToGptServer(requestDto);
        }

    }
}
