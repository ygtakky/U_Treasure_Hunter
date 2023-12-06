using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioEventChannel", menuName = "Event Channels/Audio Event Channel")]
public class AudioEventChannelSO : GenericEventChannelSO<AudioEventArgs>
{
    
}

public class AudioEventArgs : EventArgs
{
    public AudioClip audioClip;

    public AudioEventArgs(AudioClip audioClip)
    {
        this.audioClip = audioClip;
    }
}