using Starxr.SDK.AI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class ImageUploadManager : MonoBehaviour
    {
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

        public void RegisterEvents()
        {
            if (isRegisterEvent)
                return;

            imageUploadButton.onClick.AddListener(OpenFileExplorer);
        }

        /// <summary>
        /// 이벤트 해제 (명시적으로 호출)
        /// </summary>
        public void UnregisterEvents()
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

        public void OnImageUploadedHandler(string base64Data)
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
