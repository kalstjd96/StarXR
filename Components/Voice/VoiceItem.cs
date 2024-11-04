using Starxr.SDK.AI.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Starxr.SDK.AI.Components
{
    public class VoiceItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject stateObject;
        [SerializeField]
        private TextMeshProUGUI voiceIndex;
        [SerializeField]
        private TextMeshProUGUI voiceName;

        [SerializeField]
        private Button optionButton;
        [SerializeField]
        private GameObject templateObject;
        [SerializeField]
        private Button selectVoice;
        [SerializeField]
        private Button deleteVoice;

        private string voiceId;
        private Action<string> onDelete;
        private Action<string> onSelect;

        public void Initialize(VoiceDto voice, int index, Action<string> onSelect, Action<string> onDelete)
        {
            voiceIndex.text = index.ToString();
            voiceName.text = voice.name;

            voiceId = voice.voice_id;
            this.onSelect = onSelect;
            this.onDelete = onDelete;

            optionButton.onClick.AddListener(OnOptionPanelHandler);
            selectVoice.onClick.AddListener(OnSelectButtonClick);
            deleteVoice.onClick.AddListener(OnDeleteButtonClick);

            templateObject.SetActive(false);
        }

        private void OnOptionPanelHandler()
        {
            templateObject.SetActive(!templateObject.activeSelf);
        }

        private void ToggleVoiceObject(bool isActive)
        {
            stateObject.SetActive(isActive);
        }


        private void OnSelectButtonClick()
        {
            if (VoiceManager.CurrentlySelectedItem != null && VoiceManager.CurrentlySelectedItem != this)
            {
                VoiceManager.CurrentlySelectedItem.ToggleVoiceObject(false);
            }

            ToggleVoiceObject(true);
            VoiceManager.CurrentlySelectedItem = this;

            string id = voiceId.Trim();
            onSelect?.Invoke(id);

            OnOptionPanelHandler();
        }

        private void OnDeleteButtonClick()
        {
            string id = voiceId.Trim();
            onDelete?.Invoke(id);
            ToggleVoiceObject(false);
            OnOptionPanelHandler();
            if (VoiceManager.CurrentlySelectedItem == this)
            {
                VoiceManager.CurrentlySelectedItem = null;
            }
        }
    }
}
