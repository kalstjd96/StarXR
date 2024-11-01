using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class ChatUIElements : MonoBehaviour
    {
        [Serializable]
        public class ChatPanelUI
        {
            public GameObject RootPanel;

            [Header("채팅 패널 관련")]
            [Space(5)]
            public GameObject Panel;
            public GameObject UserBubblePrefab;
            public GameObject NpcBubblePrefab;
            
            public Transform Content;
            public ScrollRect ScrollRect;
            public Button CloseChatButton;
            
            [Header("InputField 관련")]
            [Space(5)]
            public GameObject ImagePrefab;
            public TMP_InputField UserInputField;
            public Transform ImageContent;
            
            public Button ImageUploadButton; 
            public Button SendMessageButton; 
            public Button OpenMicButton; 

            public RectTransform InputFieldContentRect;
            public RectTransform ContainerRect;
            public RectTransform ImageContainer; //이미지와 텍스트를 포함한 컨테이너
            
        }

        [Serializable]
        public class MicPanelUI
        {
            [Header("마이크 패널 관련")]
            [Space(5)]
            public GameObject MicPanel;
            public Animator MicAnimator;
            public Button MicButton;
            public Button MicPanelCloseButton;
            public string MicAnimatorParameterName;
        }

        [Serializable]
        public class VoicePanelUI
        {
            [Header("음성 리스트 패널")]
            [Space(5)]
            public Transform VoiceListContainer;
            public GameObject VoiceItemPrefab;               

            [Header("음성 등록 패널")]
            public GameObject PanelVoiceRegisterPanel;
            public GameObject PanelVoiceStep1;
            public GameObject PanelVoiceStep2;               
            public GameObject PanelVoiceStep3;               

            [Header("Buttons")]
            public Button ButtonVoiceRegister;
            public Button ButtonStartRecording;
            public Button ButtonNextStep1;
            public Button ButtonNextStep2;
            public Button ButtonSaveStep3;
            public Button ButtonCloseChatPanel;
            public TMP_Dropdown MicrophoneDropdown;

            [Header("Text UI")]
            public TextMeshProUGUI TextTimer;
            public TextMeshProUGUI TextRecordingState;
            public TMP_InputField InputFieldVoiceName;
        }

        public ChatPanelUI ChatPanelUIGroup;
        public MicPanelUI MicPanelUIGroup;
        public VoicePanelUI VoicePanelUIGroup;
    }
}
