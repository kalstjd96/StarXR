using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class UserChatItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI sendChatText;
        public Image ImageComponent;
        
        public void SetItem(string chat)
        {
            if(sendChatText != null)
                sendChatText.text = chat;
        }
        public void SetImageFromBase64(string base64Data)
        {
            byte[] imageBytes = System.Convert.FromBase64String(base64Data);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes); 

            ImageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            ImageComponent.preserveAspect = true;

        }
    }
}
