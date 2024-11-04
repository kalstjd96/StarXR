
using UnityEngine;

namespace Starxr.SDK.AI.Components
{
    public class CanvasInteractionHandler : MonoBehaviour
    {
        private readonly GameObject canvas;

        public CanvasInteractionHandler(GameObject canvas)
        {
            this.canvas = canvas;
        }

        public void Interact(bool isOn)
        {
            canvas.SetActive(isOn);
        }
    }
}
