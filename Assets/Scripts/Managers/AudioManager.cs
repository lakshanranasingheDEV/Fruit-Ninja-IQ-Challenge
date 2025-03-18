using Yunash.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

        [SerializeField] AudioMixer SoundaudioMixer;
        [SerializeField] AudioMixer SFXaudioMixer;
        [SerializeField] GameObject SoundON;
        [SerializeField] GameObject SoundOFF;
        [SerializeField] GameObject SFXON;
        [SerializeField] GameObject SFXOFF;
        
        
        private AudioData audioData;

        private void Start()
        {
            audioData = GameManager.Instance.DataService.AudioData;

            // Load saved sound setting
            if (PlayerPrefs.HasKey("SoundState"))
            {
                int soundState = PlayerPrefs.GetInt("SoundState");
                if (soundState == 0)
                {
                    SoundOnB(); 
                    SFXdOnB();
                }
                else
                {
                    SoundOFFB();
                    SFXOFFB();
                }
            }
        }



        public void SetSound(float sound)
        {
            SoundaudioMixer.SetFloat("sound", sound);
        }

        public void SoundOnB()
        {
            SoundaudioMixer.SetFloat("Sound", -80f); 
            SoundON.SetActive(false);
            SoundOFF.SetActive(true);

            PlayerPrefs.SetInt("SoundState", 0); 
            PlayerPrefs.Save();
        }

        public void SoundOFFB()
        {
            SoundaudioMixer.SetFloat("Sound", -10f); 
            SoundOFF.SetActive(false);
            SoundON.SetActive(true);

            PlayerPrefs.SetInt("SoundState", 1); 
            PlayerPrefs.Save();
        }


        public void SetSFX(float SFX)
        {
            SFXaudioMixer.SetFloat("SFX", SFX);
        }

        public void SFXdOnB()
        {
            SFXaudioMixer.SetFloat("SFX", -80f); 
            SFXON.SetActive(false);
            SFXOFF.SetActive(true);

            PlayerPrefs.SetInt("SFXState", 0); 
            PlayerPrefs.Save();
        }

        public void SFXOFFB()
        {
            SFXaudioMixer.SetFloat("SFX", -10f); 
            SFXOFF.SetActive(false);
            SFXON.SetActive(true);

            PlayerPrefs.SetInt("SFXState", 1); 
            PlayerPrefs.Save();
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
            // Stop the appropriate audio based on type
            if (audioType == AudioType.ButtonClick)
            {
                soundsAudioSource.Stop(); // Stop all sound effects
            }
            else if (audioType == AudioType.IdleBackgroundMusic || audioType == AudioType.InGameBackgroundMusic || audioType == AudioType.LevelComplete)
            {
                musicAudioSource.Stop(); // Stop background music
            }
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
