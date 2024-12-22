using System;
using System.Collections;
using System.Collections.Generic;
using Yunash.Audio;
using UnityEngine;
using AudioType = Yunash.Audio.AudioType;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/Audio Data")]
public class AudioData : ScriptableObject
{
    [SerializeField] private List<AudioClipData> clipData;
    private Dictionary<Yunash.Audio.AudioType, AudioClip> audioClipByType;

    public List<AudioClipData> ClipData => clipData;
    public bool TryGetClip(AudioType audioType, out AudioClip clip)
    {
        if(audioClipByType == null)
        {
            audioClipByType = new Dictionary<AudioType, AudioClip>();
            foreach(var data in clipData) 
                audioClipByType.Add(data.Type, data.AudioClip);
        }

        return audioClipByType.TryGetValue(audioType, out clip);
    }
}

[Serializable]
public struct AudioClipData
{
    public AudioType Type;
    public AudioClip AudioClip;
}