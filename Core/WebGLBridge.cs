using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Starxr.SDK.AI.Core
{
    internal static class WebGLBridge
    {

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Recording(string AICanvasName);

    [DllImport("__Internal")]
    private static extern void StopRecording();
    
    [DllImport("__Internal")]
    private static extern void OpenFileExplorerJS(); 

    [DllImport("__Internal")]
    private static extern void ImageUpload(string AICanvasName); 

    [DllImport("__Internal")]
    private static extern void VoiceRegister(string unityObjectName, string deviceId);

    [DllImport("__Internal")]
    public static extern void SetGameObjectName(string objectName);

    [DllImport("__Internal")]
    public static extern void InquiryMicrophones();

    [DllImport("__Internal")]
    private static extern void StopRegister();
#endif

        public static string OnRecordingComplete(string base64Wav)
        {
            var request = new
            {
                config = new
                {
                    languageCode = "ko-KR",
                    useEnhanced = true,
                },
                audio = new
                {
                    content = base64Wav
                }
            };
            string requestJson = JsonConvert.SerializeObject(request);
            return requestJson;
        }

        public static void StartRecording(string AICanvasName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Recording(AICanvasName);
#endif
        }

        public static void EndRecording()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        StopRecording();
#endif
        }

        public static void OpenFileExplorer()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            OpenFileExplorerJS();
#endif
        }

        public static void ImageUploadLocationSetting(string AICanvasName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ImageUpload(AICanvasName);
#endif
        }

        public static void StartVoiceRecording(string objectName, string deviceId)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            VoiceRegister(objectName, deviceId);
#endif
        }

        public static void InquiryMicrophonesSetting(string objectName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetGameObjectName(objectName);
#endif
        }

        public static void InquiryMicrophone()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            InquiryMicrophones();
#endif
        }

        public static void StopVoiceRegister()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StopRegister();
#endif
        }

    }
}
