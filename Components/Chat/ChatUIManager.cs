using Starxr.SDK.AI.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class ChatUIManager : MonoBehaviour
    {
        //채팅 패널 관련
        private GameObject chatRootPanel;
        private Transform chatContent;
        private ScrollRect chatScrollRect;
        private GameObject userBubblePrefab;
        private GameObject npcBubblePrefab;

        //InputField 및 이미지 관련
        private RectTransform inputFieldContentRect;
        private RectTransform containerRect;
        private Transform imageContent;
        private RectTransform imageContainer;
        private GameObject imagePrefab;
        private readonly List<ImageItem> imageItems = new();

        private Button closeChatButton;
        private Button sendMessageButton;
        private TMP_InputField userInputField;

        // 높이 조절 관련 상수 및 상태
        private const float MinHeight = 64f;
        private const float MaxHeight = 250f;
        private const float IncreaseHeight = 60f;
        private bool isHeightIncreased;

        public event Action OnImageDeleted;
        private bool isRegisterEvent = false;


        public event Action OnClosePanel;
        public event Action<string> OnSendMessage;


        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="chatPanelUI"></param>
        public void Initialize(ChatUIElements.ChatPanelUI chatPanelUI)
        {
            chatRootPanel = chatPanelUI.RootPanel;
            chatContent = chatPanelUI.Content;
            chatScrollRect = chatPanelUI.ScrollRect;
            inputFieldContentRect = chatPanelUI.InputFieldContentRect;
            containerRect = chatPanelUI.ContainerRect;
            userBubblePrefab = chatPanelUI.UserBubblePrefab;
            npcBubblePrefab = chatPanelUI.NpcBubblePrefab;
            imagePrefab = chatPanelUI.ImagePrefab;
            imageContent = chatPanelUI.ImageContent;
            imageContainer = chatPanelUI.ImageContainer;
            closeChatButton = chatPanelUI.CloseChatButton;
            sendMessageButton = chatPanelUI.SendMessageButton;
            userInputField = chatPanelUI.UserInputField;
            isHeightIncreased = false;
        }

        public void RegisterEvents()
        {
            if (isRegisterEvent)
                return;
            closeChatButton.onClick.AddListener(OnClosePanelHandler);
            sendMessageButton.onClick.AddListener(OnSendMessageHandler);

            isRegisterEvent = true;
        }

        private void OnClosePanelHandler()
        {
            ClearChatPanel();
            UpdateContainerHeight();
            userInputField.text = "";
            OnClosePanel?.Invoke();
        }

        public void OnSendMessageHandler()
        {
            string message = userInputField.text.Trim();
            userInputField.text = "";

            OnSendMessage?.Invoke(message);
        }


        /// <summary>
        /// 이벤트 해제 (명시적으로 호출)
        /// </summary>
        public void UnregisterEvents()
        {
            if (!isRegisterEvent)
                return;

            isRegisterEvent = false;
        }

        /// <summary>
        /// 채팅 패널 Clear
        /// </summary>
        public void ClearChatPanel()
        {
            chatRootPanel.SetActive(false);
            DestroyAllChildren(chatContent);
        }

        /// <summary>
        /// inputField창에 이미지 띄우기
        /// </summary>
        /// <param name="sprite"></param>
        public void AddImage(Sprite sprite)
        {
            ImageItem imageInstance = Instantiate(imagePrefab, imageContent).GetComponent<ImageItem>();
            imageItems.Add(imageInstance);
            imageInstance.OnDelete = DeleteImage;
            imageInstance.GetComponent<Image>().sprite = sprite;
            imageInstance.GetComponent<Image>().preserveAspect = true;

            AdjustContainerHeight();
        }

        /// <summary>
        /// inputField창에 있는 이미지 지우기
        /// </summary>
        /// <param name="deletedImageItem"></param>
        private void DeleteImage(ImageItem deletedImageItem)
        {
            imageItems.Remove(deletedImageItem);
            DestroyImmediate(deletedImageItem.gameObject);

            if (imageItems.Count == 0)
            {
                AdjustContainerHeight(true);
                OnImageDeleted?.Invoke();
            }
        }

        /// <summary>
        /// inputField창에 띄운 이미지들 초기화
        /// </summary>
        public void ClearImages()
        {
            if (imageContent.childCount != 0)
            {
                DestroyAllChildren(imageContent);
            }

            //OnImageDeleted?.Invoke();
            AdjustContainerHeight(reset: true);
        }

        /// <summary>
        /// inputField 내에서 이미지가 들어가는 구간의 높이 조절
        /// </summary>
        /// <param name="reset"></param>
        private void AdjustContainerHeight(bool reset = false)
        {
            if (reset)
            {
                imageContainer.sizeDelta = new Vector2(imageContainer.sizeDelta.x, Mathf.Max(0f, imageContainer.sizeDelta.y - IncreaseHeight));
                isHeightIncreased = false;
            }
            else if (!isHeightIncreased)
            {
                imageContainer.sizeDelta = new Vector2(imageContainer.sizeDelta.x, imageContainer.sizeDelta.y + IncreaseHeight);
                isHeightIncreased = true;
            }

            UpdateContainerHeight();
        }


        private void DestroyAllChildren(Transform parent)
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);
        }
       
        public void AddUserMessage(string message, string imageData = null)
        {
            if (string.IsNullOrEmpty(message) && imageData == null)
                return;

            CreateUserMessageBubble(chatContent, message, imageData);
            UpdateChatScroll();
        }

        public void AddGptMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            CreateGptMessageBubble(chatContent, message);
            UpdateChatScroll();
        }

        private void CreateUserMessageBubble(Transform parent, string message, string imageData = null)
        {
            GameObject messageBubble = Instantiate(userBubblePrefab, parent);
            var chatItem = messageBubble.GetComponent<UserChatItem>();
            chatItem.SetItem(message);

            if (!string.IsNullOrEmpty(imageData))
            {
                chatItem.SetImageFromBase64(imageData);
            }
        }

        private void CreateGptMessageBubble(Transform parent, string message)
        {
            GameObject messageBubble = Instantiate(npcBubblePrefab, parent);
            messageBubble.GetComponent<NPCChatItem>().SetItem(message);
        }

        /// <summary>
        /// 스크롤 맨 아래로 내리기
        /// </summary>
        public void UpdateChatScroll()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatScrollRect.GetComponent<RectTransform>());
            chatScrollRect.normalizedPosition = Vector2.zero;
        }

        /// <summary>
        /// inputField의 높이
        /// </summary>
        public void UpdateContainerHeight()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(inputFieldContentRect);
            float height = inputFieldContentRect.sizeDelta.y;
            containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, Mathf.Clamp(height, MinHeight, MaxHeight));
        }

        /// <summary>
        /// gpt에 메시지 전달
        /// </summary>
        /// <param name="message"></param>
        /// <param name="imageData"></param>
        public void SendMessageGPT(string message, string imageData = null)
        {
            AddUserMessage(message, imageData);
            ClearImages();
            UpdateContainerHeight();
        }
    }
}
