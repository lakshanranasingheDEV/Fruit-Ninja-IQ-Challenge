using UnityEngine;
using UnityEngine.UI;
using Yunash.Audio;

public class ButtonSoundHandler : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {

        audioManager = FindObjectOfType<AudioManager>();


        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }

    private void PlayButtonClickSound()
    {

        if (audioManager != null)
        {
            audioManager.PlayAudio(Yunash.Audio.AudioType.ButtonClick);
        }
    }
}