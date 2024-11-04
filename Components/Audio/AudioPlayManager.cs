using Cysharp.Threading.Tasks;
using Starxr.SDK.AI.Core;
using System;
using UnityEngine;
namespace Starxr.SDK.AI.Components
{
    public class AudioPlayManager : StarxrComponentBase
    {
        public override string Title => "AI 채팅 오디오 관리 매니저";
        public override string Tooltip => "AI 채팅 내 오디오 정보를 관리하는 매니저입니다";

        public AudioSource AudioSource { get; private set; }

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        public void Initialize(AudioSource audioSource)
        {
            this.AudioSource = audioSource ?? throw new ArgumentNullException(nameof(audioSource));
        }

        public void PlayAudio(string audioData)
        {
            if (string.IsNullOrEmpty(audioData))
                return;

            try
            {
                byte[] audioDataBytes = Convert.FromBase64String(audioData);
                AudioClip clip = WavUtility.ToAudioClip(audioDataBytes);
                PlayAudioClip(clip).Forget();
            }
            catch (FormatException ex)
            {
                Debug.LogError($"오디오 데이터 변환 실패: {ex.Message}");
            }
        }

        public async UniTaskVoid PlayAudioClip(AudioClip audioClip)
        {
            AudioSource.clip = audioClip;
            AudioSource.Play();
            await UniTask.Delay((int)(audioClip.length * 1000));
            
            ResetAudioSource();
        }

        public void ResetAudioSource()
        {
            AudioSource.Stop();
            AudioSource.clip = null; 
        }
    }
}
