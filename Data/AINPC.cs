using Starxr.SDK.AI.Components;
using Starxr.SDK.AI.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Starxr.SDK.AI.Data
{
    public class AINPC : MonoBehaviour
    {
        public AICategory Category;
        public LANGUAGE Language;
        public GameObject ChattingCanvas;

        [SerializeField] private Animator npcAnimator;
        [SerializeField] private string animatorParameterBoolName;

        private CanvasInteractionHandler interactionHandler;
        private AnimatorHandler animationHandler;

        public Dictionary<AICategory, string> NPCIDDictonary { get; private set; }
        public Dictionary<LANGUAGE, string> LanguageDictonary { get; private set; }

        private void Awake()
        {
            interactionHandler = new CanvasInteractionHandler(ChattingCanvas);
            animationHandler = new AnimatorHandler(npcAnimator, animatorParameterBoolName);

            NPCIDDictonary = GPTSettings.Instance.GetNPCIDDictonary();
            LanguageDictonary = GPTSettings.Instance.GetLanguageDictonary();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                interactionHandler.Interact(true);
                animationHandler.PlayAnimation();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                interactionHandler.Interact(false);
                animationHandler.StopAnimation();
            }
        }
    }

}
