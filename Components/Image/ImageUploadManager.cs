using Starxr.SDK.AI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class ImageUploadManager : StarxrComponentBase
    {
        public override string Title => "AI 채팅 이미지 관리 매니저";
        public override string Tooltip => "AI 채팅 내 이미지 정보를 관리하는 매니저로" +
            "입력창 및 채팅창에 이미지를 나타내고 서버에 이미지 정보를 전달하는 역할을 합니다.";

        public string ServerImageData { get; private set; } //서버에 보낼 이미지 Data
        public string InternalImageData { get; private set; } //내부에 사용될 이미지 Data
        public event Action<Sprite> OnImageUploaded;

        private Button imageUploadButton;
        private bool isRegisterEvent = false;
        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        public void Initialize(Button imageUploadButton)
        {
            WebGLBridge.ImageUploadLocationSetting(gameObject.name);
            this.imageUploadButton = imageUploadButton;
        }

        public void RegisterUIEventHandlers()
        {
            if (isRegisterEvent)
                return;

            imageUploadButton.onClick.AddListener(OpenFileExplorer);
        }

        /// <summary>
        /// 이벤트 해제 (명시적으로 호출)
        /// </summary>
        public void UnregisterUIEventHandlers()
        {
            if (!isRegisterEvent)
                return;

            imageUploadButton.onClick.RemoveListener(OpenFileExplorer);
            isRegisterEvent = false;
        }

        public void OpenFileExplorer()
        {
            WebGLBridge.OpenFileExplorer();
        }

        public void JslibImageUpload(string base64Data)
        {
            ServerImageData = base64Data;
            InternalImageData = base64Data.Split(',')[1];

            Sprite sprite = ConvertBase64ToSprite(InternalImageData);
            OnImageUploaded?.Invoke(sprite);
        }

        public Sprite ConvertBase64ToSprite(string base64Data)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            Texture2D texture = new Texture2D(80, 80);
            texture.LoadImage(imageBytes);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public void ClearImageData()
        {
            ServerImageData = null;
            InternalImageData = null;
        }
    }

}
