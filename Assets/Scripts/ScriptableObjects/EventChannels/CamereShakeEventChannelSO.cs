using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Shake Event Channel", menuName = "Event Channels/Camera Shake Event Channel")]
public class CameraShakeEventChannelSO : GenericEventChannelSO<CameraShakeEventArgs>
{
    
}

public class CameraShakeEventArgs : EventArgs
{
    public float Amplitude;
    public float Frequency;
    public float Duration;
    
    public CameraShakeEventArgs(float amplitude, float frequency, float duration)
    {
        Amplitude = amplitude;
        Frequency = frequency;
        Duration = duration;
    }
}