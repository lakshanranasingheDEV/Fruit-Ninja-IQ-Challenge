using Yunash.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yunash.Audio
{
    public interface IAudioService
    {
        void PlayAudio(AudioType audioType);
        void StopAudio(AudioType audioType);
        void MuteSounds(bool isMute);
        void MuteMusic(bool isMute);
    }

    public class AudioManager : MonoBehaviour, IAudioService
    {
        [SerializeField] AudioSource soundsAudioSource;
        [SerializeField] AudioSource musicAudioSource;
        private AudioData audioData;

        private void Start()
        {
            audioData = GameManager.Instance.DataService.AudioData;
        }
        public void MuteMusic(bool isMute)
        {
            musicAudioSource.mute = isMute;
        }

        public void MuteSounds(bool isMute)
        {
            soundsAudioSource.mute = isMute;
        }

        public void PlayAudio(AudioType audioType)
        {
            audioData.TryGetClip(audioType, out AudioClip clip);

            if (clip == null)
                return;

            if (audioType == AudioType.ButtonClick)
            {
                soundsAudioSource.clip = clip;
                soundsAudioSource.Play();
            }
            else if (audioType == AudioType.IdleBackgroundMusic)
            {
                musicAudioSource.clip = clip;
                musicAudioSource.Play();
            }
        }

        public void StopAudio(AudioType audioType)
        {
        }
    }

    public enum AudioType
    {
        IdleBackgroundMusic,
        InGameBackgroundMusic,
        EnterGame,
        PanelOpen,
        PanelClose,
        ButtonClick,
        RewardPopup,
        LetterSelect,
        Error,
        Success,
        MiscAction,
        LevelComplete,
        ProgressBarFill,
        ProgressBarComplete
    }
}
