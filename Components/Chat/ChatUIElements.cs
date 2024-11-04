using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class ChatUIElements : StarxrComponentBase
    {
        public override string Title => "AI 채팅 UI변수 관리 매니저";
        public override string Tooltip => "AI 채팅 내 TTS 기능으로 사용하고자 하는 목소리를 등록하여" +
            "답변을 해당 목소리로 들을 수 있는 기눙, 목소리 정보를 관리하는 매니저입니다";

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
            public List<GameObject> PanelVoiceStepList;     

            [Header("Buttons")]
            public Button ButtonVoiceRegister;
            public Button ButtonCloseChatPanel;

            // Step 1 페이지 
            public Button ButtonCheckVoice;
            public Button ButtonNextStep1;
            public TMP_Dropdown MicrophoneDropdown;

            //Step 2 페이지
            public Button ButtonStartRecording;
            public Button ButtonNextStep2;

            //Step 3 페이지
            public Button ButtonSaveStep3;

            [Header("Text UI")]
            public TextMeshProUGUI TextTimer;
            public TextMeshProUGUI TextRecordingState;
            public TextMeshProUGUI TextMicState;
            public TextMeshProUGUI TextVoiceScript;
            public TMP_InputField InputFieldVoiceName;
            public Image MicImage;
        }

        public ChatPanelUI ChatPanelUIGroup;
        public MicPanelUI MicPanelUIGroup;
        public VoicePanelUI VoicePanelUIGroup;
    }
}
