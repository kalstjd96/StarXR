using Starxr.SDK.AI.Data;
using Starxr.SDK.AI.Interfaces;
using System.Text.RegularExpressions;

namespace Starxr.SDK.AI.Components
{
    public class MessageSender
    {
        private readonly IGptCommunicationService _gptCommunicationManager;
        private readonly ChatUIManager chatUIManager;
        private readonly AudioPlayManager audioPlayManager;
        private readonly ImageUploadManager imageUploadManager;
        private readonly AINPC aiData;
        
        private bool useTTS = false;

        public MessageSender(
            IGptCommunicationService gptCommunicationManager,
            ChatUIManager chatUIManager,
            ImageUploadManager imageUploadManager,
            AudioPlayManager audioPlayManager,
            AINPC aiData)
        {
            _gptCommunicationManager = gptCommunicationManager;
            this.chatUIManager = chatUIManager;
            this.imageUploadManager = imageUploadManager;
            this.audioPlayManager = audioPlayManager;
            this.aiData = aiData;
        }


        public async void SendMessage(string userAudioText, string chatID, string voiceID = null, string imageData = null)
        {
            if (string.IsNullOrEmpty(userAudioText)) return;

            useTTS = !string.IsNullOrEmpty(voiceID);

            var requestDto = new RequestGptServerDto(
                aICategoryNPCID: aiData.NPCIDDictonary[aiData.Category],
                aIVoiceID: voiceID,
                chatId: chatID,
                queryText: userAudioText,
                streaming: false,
                language: aiData.LanguageDictonary[aiData.Language],
                useTTS: useTTS,
                queryImage: imageData
            );

            var response = await _gptCommunicationManager.SendMessageToGptServer(requestDto);

            if (response == null) return;

            string answer = Regex.Unescape(Regex.Escape(response.Answer));
            string audioData = string.IsNullOrEmpty(response.VoiceData) ? null : response.VoiceData;

            chatUIManager.AddGptMessage(answer);

            if (!string.IsNullOrEmpty(imageData))
                imageUploadManager.ClearImageData();

            if (!string.IsNullOrEmpty(audioData))
                audioPlayManager.PlayAudio(audioData);
        }
    }

}
