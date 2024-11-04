using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Core;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using System;
using UnityEngine;

namespace Starxr.SDK.AI.Components
{
    public class AIChatServiceManager : StarxrComponentBase
    {
        public override string Title => "AI 채팅 관리 매니저";
        public override string Tooltip => "AI 채팅에 사용되는 모든 기능을 " +
            " 조율하고 호출하는 중앙 컨트롤러입니다.";

        public AINPC AiData;

        #region Private Fields
        //수정 필요! 
        [SerializeField] 
        private AudioSource backGroundSound;
        [SerializeField]
        private GameObject messageChat;


        [SerializeField] 
        private ChatUIElements chatUIElements;

        private MicManager micManager;
        private ChatUIManager chatUIManager;
        private AudioPlayManager audioPlayManager;
        private ImageUploadManager imageUploadManager;
        private VoiceManager voiceManager;
        private MessageSender messageSender;
        private IGptCommunicationService _gptCommunicationManager;
        private IGoogleCloudService _googleCloudServiceManager;

        private AudioSource audioSource;
        private string chatID;
        private string voiceID = null;
        private bool isInitialized = false;
        #endregion

        #region Unity Method
        private async void OnEnable()
        {
            if (backGroundSound != null)
                backGroundSound.mute = true;

            if (messageChat != null)
                messageChat.SetActive(false);

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
            if(backGroundSound != null)
                backGroundSound.mute = false;

            if (messageChat != null)
                messageChat.SetActive(true);

            if (!string.IsNullOrEmpty(chatID))
            {
                _gptCommunicationManager.DeleteChatID(chatID);
                chatID = null;
            }

            UnsubscribeChatEvents();
            audioPlayManager.ResetAudioSource();
            imageUploadManager.ClearImageData();
            voiceID = null;
        }
        #endregion

        
        private T GetManager<T>() where T : class
        {
            var manager = ServiceLocator.Get<T>();
            if (manager == null)
            {
                throw new Exception($"Manager of type {typeof(T).Name} not found.");
            }
            return manager;
        }

        private async UniTask InitializeComponentsAsync()
        {
            await UniTask.Yield();

            try
            {
                if (!GetComponent<AudioSource>())
                    gameObject.AddComponent<AudioSource>();

                audioSource = GetComponent<AudioSource>();

                micManager = GetManager<MicManager>();
                chatUIManager = GetManager<ChatUIManager>();
                imageUploadManager = GetManager<ImageUploadManager>();
                audioPlayManager = GetManager<AudioPlayManager>();
                voiceManager = GetManager<VoiceManager>();

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

        private void SubscribeChatEvents()
        {
            micManager.OnRecordingComplete += OnRecordingCompleteHandler;
            chatUIManager.OnSendMessage += OnSendMessageHandler;
            imageUploadManager.OnImageUploaded += chatUIManager.AddImage;
            chatUIManager.OnImageDeleted += imageUploadManager.ClearImageData;
            voiceManager.OnVoiceSelected += SetVoiceID;
            voiceManager.OnVoiceInited += SetVoiceID;

            imageUploadManager.RegisterUIEventHandlers();
            micManager.RegisterUIEventHandlers();
            chatUIManager.RegisterUIEventHandlers();
            voiceManager.RegisterUIEventHandlers();
        }

        private void UnsubscribeChatEvents()
        {
            micManager.OnRecordingComplete -= OnRecordingCompleteHandler;
            chatUIManager.OnSendMessage -= OnSendMessageHandler;
            imageUploadManager.OnImageUploaded -= chatUIManager.AddImage;
            chatUIManager.OnImageDeleted -= imageUploadManager.ClearImageData;
            voiceManager.OnVoiceSelected -= SetVoiceID;
            voiceManager.OnVoiceInited -= SetVoiceID;

            imageUploadManager.UnregisterUIEventHandlers();
            micManager.UnregisterUIEventHandlers();
            chatUIManager.UnregisterUIEventHandlers();
            voiceManager.UnregisterUIEventHandlers();
        }

        private void SetVoiceID(string voiceID)
        {
            this.voiceID = voiceID;
        }

        private async void OnRecordingCompleteHandler(string audioJson)
        {
            try
            {
                string userAudioText = await _googleCloudServiceManager.CallSTT(audioJson, 
                    GPTSettings.Instance.STTAPIUrl, GPTSettings.Instance.GoogleCloudAPIKey);
                chatUIManager.AddUserMessage(userAudioText);
                messageSender.SendMessage(userAudioText, chatID, voiceID);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing recorded audio: {ex.Message}");
            }
        }

        private void OnSendMessageHandler(string message)
        {
            chatUIManager.AddUserMessage(message, imageUploadManager.InternalImageData);
            chatUIManager.ClearImages();
            chatUIManager.UpdateContainerHeight();
            messageSender.SendMessage(message, chatID, voiceID, imageUploadManager.ServerImageData);
        }
    }

}
