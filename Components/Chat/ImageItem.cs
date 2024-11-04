using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class ImageItem : MonoBehaviour
    {
        #region Public Fields
        public Button ImageFileDeleteButton;
        public Image GetImage;

        public System.Action<ImageItem> OnDelete;
        #endregion

        public void Start()
        {
            ImageFileDeleteButton.onClick.AddListener(() => DeleteItem());
        }

        public void DeleteItem()
        {
            OnDelete?.Invoke(this);
        }
    }

}
