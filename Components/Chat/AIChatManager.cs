using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Core;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using System;
using UnityEngine;

namespace Starxr.SDK.AI.Components
{
    public class AIChatManager : MonoBehaviour
    {
        [NonSerialized]
        public string VoiceID;
        public AINPC AiData;

        [SerializeField] 
        private AudioSource SoundPlayer;
        [SerializeField] 
        private ChatUIElements chatUIElements;
        [SerializeField] 
        private AudioSource audioSource;

        private MicManager micManager;
        private ChatUIManager chatUIManager;
        private AudioPlayManager audioPlayManager;
        private ImageUploadManager imageUploadManager;
        private VoiceManager voiceManager;
        private MessageSender messageSender;
        private IGptCommunicationService _gptCommunicationManager;
        private IGoogleCloudService _googleCloudServiceManager;
        private string chatID;
        private bool isInitialized = false;

        #region Unity Method
        private async void OnEnable()
        {
            SoundPlayer.mute = true;

            if (!isInitialized)
            {
                await InitializeComponentsAsync();
                isInitialized = true;
            }

            chatID = await _gptCommunicationManager.CreateChatID();
            SubscribeChatEvents();
        }

        private void OnDisable()
        {
            SoundPlayer.mute = false;

            if (!string.IsNullOrEmpty(chatID))
            {
                _gptCommunicationManager.DeleteChatID(chatID);
                chatID = null;
            }

            UnsubscribeChatEvents();
            audioPlayManager.ResetAudioSource();
            imageUploadManager.ClearImageData();
        }
        #endregion

        #region Init & Setting

        private async UniTask InitializeComponentsAsync()
        {
            await UniTask.Yield();

            try
            {
                micManager = ServiceLocator.Get<MicManager>();
                chatUIManager = ServiceLocator.Get<ChatUIManager>();
                imageUploadManager = ServiceLocator.Get<ImageUploadManager>();
                audioPlayManager = ServiceLocator.Get<AudioPlayManager>();
                voiceManager = ServiceLocator.Get<VoiceManager>();

                micManager.Initialize(chatUIElements.MicPanelUIGroup);
                chatUIManager.Initialize(chatUIElements.ChatPanelUIGroup);
                imageUploadManager.Initialize(chatUIElements.ChatPanelUIGroup.ImageUploadButton);
                audioPlayManager.Initialize(audioSource);
                voiceManager.Initialize(chatUIElements.VoicePanelUIGroup);

                _gptCommunicationManager = new GptCommunicationService();
                _googleCloudServiceManager = new GoogleCloudService();
                messageSender = new MessageSender(_gptCommunicationManager, chatUIManager,
                    imageUploadManager, audioPlayManager, AiData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Initialization Error: {ex.Message}");
            }
        }

        public void SubscribeChatEvents()
        {
            micManager.OnRecordingComplete += OnRecordingComplete;
            chatUIManager.OnSendMessage += OnSendMessageHandler;
            imageUploadManager.OnImageUploaded += chatUIManager.AddImage;
            chatUIManager.OnImageDeleted += imageUploadManager.ClearImageData;

            imageUploadManager.RegisterEvents();
            micManager.RegisterEvents();
            chatUIManager.RegisterEvents();
            voiceManager.RegisterEvents();
        }
        public void UnsubscribeChatEvents()
        {
            micManager.OnRecordingComplete -= OnRecordingComplete;
            chatUIManager.OnSendMessage -= OnSendMessageHandler;
            imageUploadManager.OnImageUploaded -= chatUIManager.AddImage;
            chatUIManager.OnImageDeleted -= imageUploadManager.ClearImageData;

            imageUploadManager.UnregisterEvents();
            micManager.UnregisterEvents();
            chatUIManager.UnregisterEvents();
            voiceManager.UnregisterEvents();
        }

        /// <summary>
        /// 마이크 음성 녹음이 끝난 뒤
        /// </summary>
        /// <param name="audioJson">웹에서 녹화한 나의 음성 데이터</param>
        private async void OnRecordingComplete(string audioJson)
        {
            string userAudioText = await _googleCloudServiceManager.CallSTT(audioJson,
                GPTSettings.Instance.STTAPIUrl, GPTSettings.Instance.GoogleCloudAPIKey);

            chatUIManager.AddUserMessage(userAudioText);
            messageSender.SendMessage(userAudioText, chatID, VoiceID);
        }

        #endregion

        /// <summary>
        /// 텍스트로 메시지를 보냈을 때 실행되는 것 
        /// </summary>
        /// <param name="message"></param>
        public void OnSendMessageHandler(string message)
        {
            chatUIManager.AddUserMessage(message, imageUploadManager.InternalImageData);
            chatUIManager.ClearImages();
            chatUIManager.UpdateContainerHeight();
            messageSender.SendMessage(message, chatID, VoiceID, imageUploadManager.ServerImageData);
        }
    }

}
