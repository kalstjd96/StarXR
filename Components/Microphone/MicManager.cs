using Starxr.SDK.AI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class MicManager : MonoBehaviour
    {
        public event Action<string> OnRecordingComplete;
        
        private GameObject micPanel;
        private Animator micAnimator;
        private AnimatorHandler animationHandler;
        
        private Button micButton;
        private Button micPanelCloseButton;

        private string parameterName;
        private bool isRegisterEvent = false;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        public void Initialize(ChatUIElements.MicPanelUI micPanelUI)
        {
            micPanel = micPanelUI.MicPanel;
            micButton = micPanelUI.MicButton;
            micPanelCloseButton = micPanelUI.MicPanelCloseButton;
            micAnimator = micPanelUI.MicAnimator;
            parameterName = micPanelUI.MicAnimatorParameterName;
            animationHandler = new AnimatorHandler(micAnimator, parameterName);
        }

        public void RegisterEvents()
        {
            if (isRegisterEvent)
                return;

            micButton.onClick.AddListener(OnRecordingHandler);
            micPanelCloseButton.onClick.AddListener(OnClosePanelHandler);
            isRegisterEvent = true;
        }

        /// <summary>
        /// 이벤트 해제 (명시적으로 호출)
        /// </summary>
        public void UnregisterEvents()
        {
            if (!isRegisterEvent)
                return;

            micPanelCloseButton.onClick.RemoveListener(OnClosePanelHandler);
            isRegisterEvent = false;
        }


        private void OnClosePanelHandler()
        {
            WebGLBridge.EndRecording();
            if (micButton != null)
                micButton.GetComponent<Image>().color = Color.white;

            micPanel.SetActive(false);
        }

        public void OnRecordingHandler()
        {
            micPanel.SetActive(true);
            micButton.GetComponent<Image>().color = new Color(1.0f, 0.64f, 0.0f);
            WebGLBridge.StartRecording(this.gameObject.name);
        }

        public void OnVolumeThresholdExceeded(string isExceeded)
        {
            if(isExceeded == "true")
            {
                animationHandler.PlayAnimation();
            }
            else
            {
                animationHandler.StopAnimation();
            }
        }

        public void OnRecordingCompleteHandler(object base64Audio)
        {
            string audioJson = WebGLBridge.OnRecordingComplete(base64Audio.ToString());
            OnRecordingComplete?.Invoke(audioJson);
        }
    }

}
