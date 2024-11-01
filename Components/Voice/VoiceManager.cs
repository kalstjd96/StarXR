using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Core;
using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace Starxr.SDK.AI.Components
{
    public class VoiceManager : MonoBehaviour
    {
        #region Constants
        private const string MIC_NOT_DETECTED = "마이크 인식이 필요합니다.";
        private const string MIC_READY = "마이크 작동 확인 완료.";
        private const string RECORDING_STOPPED = "녹음하기";
        private const string RECORDING_IN_PROGRESS = "녹음 중지";
        #endregion

        #region Private Fields
        public Action<string> OnVoiceSelected;

        private ChatUIElements.VoicePanelUI voicePanelUI;
        private ICustomVoiceService voiceService;
        private VoiceDataDto voiceDataList;
        private Coroutine timerCoroutine;
        private bool isRecording = false;
        private TaskCompletionSource<bool> microphoneTaskCompletionSource;

        private List<MicrophoneDevice> microphoneDevices = new List<MicrophoneDevice>();
        private int pageIndex;
        private int voiceIndex;
        private string voiceName;
        private string voiceAudioData;
        private string selectedMicrophone;
        #endregion

        #region Unity Methods
        private void Awake() => ServiceLocator.Register(this);
        #endregion

        #region Initialization and Event Handling
        public void Initialize(ChatUIElements.VoicePanelUI voicePanelUI)
        {
            this.voicePanelUI = voicePanelUI;
            voiceService = new CustomVoiceService();
            voiceDataList = new VoiceDataDto();
            WebGLBridge.InquiryMicrophonesSetting(this.gameObject.name);
        }

        public void RegisterUIEventHandlers()
        {
            voicePanelUI.ButtonVoiceRegister.onClick.AddListener(ShowRegisterPanel);
            voicePanelUI.ButtonStartRecording.onClick.AddListener(StartVoiceRecording);
            voicePanelUI.ButtonNextStep1.onClick.AddListener(() => SwitchPanel(1));
            voicePanelUI.ButtonNextStep2.onClick.AddListener(() => SwitchPanel(2));
            voicePanelUI.ButtonSaveStep3.onClick.AddListener(CompleteVoiceRegistration);
            voicePanelUI.ButtonCloseChatPanel.onClick.AddListener(() => TogglePanel(voicePanelUI.PanelVoiceRegisterPanel, false));
            voicePanelUI.InputFieldVoiceName.onValueChanged.AddListener(OnInputFieldValueChanged);

            RetrieveVoiceList();
        }

        public void UnregisterUIEventHandlers()
        {
            voicePanelUI.ButtonVoiceRegister.onClick.RemoveListener(ShowRegisterPanel);
            voicePanelUI.ButtonStartRecording.onClick.RemoveListener(StartVoiceRecording);
            voicePanelUI.ButtonNextStep1.onClick.RemoveListener(() => SwitchPanel(1));
            voicePanelUI.ButtonNextStep2.onClick.RemoveListener(() => SwitchPanel(2));
            voicePanelUI.ButtonSaveStep3.onClick.RemoveListener(CompleteVoiceRegistration);
            voicePanelUI.ButtonCloseChatPanel.onClick.RemoveListener(() => TogglePanel(voicePanelUI.PanelVoiceRegisterPanel, false));
            voicePanelUI.InputFieldVoiceName.onValueChanged.RemoveListener(OnInputFieldValueChanged);

            ClearVoiceList();
        }
        #endregion

        #region Custom Voice 등록
        private void ResetInformation()
        {
            ResetTimer();
            SetButtonInteractable(false);
            voicePanelUI.InputFieldVoiceName.text = string.Empty;
            pageIndex = 0;
        }

        private async void ShowRegisterPanel()
        {
            ResetInformation();
            microphoneTaskCompletionSource = new TaskCompletionSource<bool>();
            InquiryMicrophone();

            await microphoneTaskCompletionSource.Task;

            TogglePanel(voicePanelUI.PanelVoiceRegisterPanel, true);
            ToggleStepPanels();
        }

        private void SwitchPanel(int nextPageIndex)
        {
            if (IsValidPageIndex(nextPageIndex))
            {
                TogglePanel(voicePanelUI.PanelVoiceStepList[pageIndex], false);
                pageIndex = nextPageIndex;
                TogglePanel(voicePanelUI.PanelVoiceStepList[pageIndex], true);
            }
        }

        private void TogglePanel(GameObject panel, bool state) => panel.SetActive(state);

        private void ResetTimer() => voicePanelUI.TextTimer.text = "00:00";

        private void InquiryMicrophone() => WebGLBridge.InquiryMicrophone();

        public void OnReceiveMicrophoneList(string jsonMicList)
        {
            var micList = JsonUtility.FromJson<MicrophoneList>(jsonMicList);
            microphoneDevices = micList.devices;

            if (microphoneDevices == null || microphoneDevices.Count == 0)
            {
                SetMicrophoneUIState(false, MIC_NOT_DETECTED);
                microphoneTaskCompletionSource.SetResult(false);
                return;
            }

            SetMicrophoneUIState(true, MIC_READY);
            PopulateMicrophoneDropdown();
            microphoneTaskCompletionSource.SetResult(true);
        }

        private void SetMicrophoneUIState(bool isEnabled, string stateText)
        {
            voicePanelUI.MicrophoneDropdown.interactable = isEnabled;
            voicePanelUI.ButtonCheckVoice.interactable = isEnabled;
            voicePanelUI.ButtonNextStep1.interactable = isEnabled;
            voicePanelUI.TextMicState.text = stateText;
            voicePanelUI.MicImage.color = isEnabled ? new Color(1.0f, 0.64f, 0.0f) : Color.gray;
        }

        private void PopulateMicrophoneDropdown()
        {
            var micNames = microphoneDevices.Select(mic => mic.label).ToList();
            voicePanelUI.MicrophoneDropdown.ClearOptions();
            voicePanelUI.MicrophoneDropdown.AddOptions(micNames);
            voicePanelUI.MicrophoneDropdown.onValueChanged.AddListener(OnMicrophoneSelected);
        }

        private void OnMicrophoneSelected(int index)
        {
            if (IsValidMicrophoneIndex(index))
            {
                selectedMicrophone = microphoneDevices[index].deviceId;
            }
        }

        private void StartVoiceRecording()
        {
            if (isRecording)
            {
                StopVoiceRecording();
                return;
            }

            isRecording = true;
            voicePanelUI.TextRecordingState.text = RECORDING_IN_PROGRESS;
            ResetTimer();
            timerCoroutine = StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            int seconds = 0;
            while (isRecording)
            {
                yield return new WaitForSeconds(1);
                seconds++;
                voicePanelUI.TextTimer.text = $"{seconds / 60:D2}:{seconds % 60:D2}";
            }
        }

        private void StopVoiceRecording()
        {
            isRecording = false;
            voicePanelUI.TextRecordingState.text = RECORDING_STOPPED;
            WebGLBridge.StopVoiceRegister();
            StopAndResetTimer();
        }

        private void StopAndResetTimer()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
            ResetTimer();
            voicePanelUI.ButtonNextStep2.interactable = false;
        }

        private async void CompleteVoiceRegistration()
        {
            voiceName = voicePanelUI.InputFieldVoiceName.text.Trim();
            await RegisterVoice(voiceName, voiceAudioData, voicePanelUI.TextVoiceScript.text.Trim());
            TogglePanel(voicePanelUI.PanelVoiceRegisterPanel, false);
            pageIndex = 0;
        }

        private void OnInputFieldValueChanged(string input)
        {
            voicePanelUI.ButtonSaveStep3.interactable = !string.IsNullOrEmpty(input.Trim());
        }

        private void OnVoiceRecordingComplete(string base64Audio)
        {
            voiceAudioData = base64Audio;
            StopVoiceRecording();
            voicePanelUI.ButtonNextStep2.interactable = true;
        }

        private async UniTask RegisterVoice(string name, string audioData, string script)
        {
            try
            {
                await voiceService.RegisterVoiceID(name, audioData, script);
                RetrieveVoiceList();
            }
            catch (RestApiResponseException ex)
            {
                Debug.LogError($"목소리 등록 실패: {ex.Message}");
            }
        }
        #endregion

        #region Custom Voice 조회 및 삭제, 선택
        public async void RetrieveVoiceList()
        {
            try
            {
                voiceIndex = 0;
                voiceDataList = await voiceService.GetVoiceList();
                DisplayVoiceList(voiceDataList);
            }
            catch (RestApiResponseException ex)
            {
                Debug.LogError($"리스트 조회 실패: {ex.Message}");
            }
        }

        private void DisplayVoiceList(VoiceDataDto voiceDataList)
        {
            foreach (var voice in voiceDataList.voice_list)
            {
                var voiceItemObject = Instantiate(voicePanelUI.VoiceItemPrefab, voicePanelUI.VoiceListContainer);
                var voiceItem = voiceItemObject.GetComponent<VoiceItem>();
                voiceItem.Initialize(voice, ++voiceIndex, id => OnVoiceSelected?.Invoke(id), async id => await DeleteVoice(id));
            }
        }

        private async UniTask DeleteVoice(string voiceID)
        {
            try
            {
                await voiceService.DeleteVoiceID(voiceID);
                RetrieveVoiceList();
            }
            catch (RestApiResponseException ex)
            {
                Debug.LogError($"목소리 삭제 실패: {ex.Message}");
            }
        }

        private void ClearVoiceList()
        {
            foreach (Transform child in voicePanelUI.VoiceListContainer)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion

        #region Utility Methods
        private bool IsValidPageIndex(int index) => index >= 0 && index < voicePanelUI.PanelVoiceStepList.Count;
        private bool IsValidMicrophoneIndex(int index) => index >= 0 && index < microphoneDevices.Count;
        private void SetButtonInteractable(bool state)
        {
            voicePanelUI.ButtonNextStep1.interactable = state;
            voicePanelUI.ButtonNextStep2.interactable = state;
            voicePanelUI.ButtonSaveStep3.interactable = state;
        }
        #endregion

        [Serializable]
        public class MicrophoneDevice
        {
            public string label;
            public string deviceId;
        }

        [Serializable]
        public class MicrophoneList
        {
            public List<MicrophoneDevice> devices;
        }
    }
}
