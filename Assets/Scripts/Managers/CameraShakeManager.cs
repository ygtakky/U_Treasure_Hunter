using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraShakeManager : MonoBehaviour
{
    [SerializeField] private CameraShakeEventChannelSO cameraShakeEventChannel;
    
    private CinemachineImpulseSource impulseSource;
    
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        impulseSource.m_DefaultVelocity = new Vector3(-0.1f, -0.1f);
    }
    
    private void OnEnable()
    {
        cameraShakeEventChannel.OnEventRaised += CameraShakeEventChannelSO_OnEventRaised;
    }
    
    private void OnDisable()
    {
        cameraShakeEventChannel.OnEventRaised -= CameraShakeEventChannelSO_OnEventRaised;
    }
    
    private void CameraShakeEventChannelSO_OnEventRaised(object sender, CameraShakeEventArgs e)
    {
        ShakeCamera(e.Amplitude, e.Frequency, e.Duration);
    }
    
    private void ShakeCamera(float amplitude, float frequency, float duration)
    {
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
        impulseSource.m_ImpulseDefinition.m_FrequencyGain = frequency;
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
        impulseSource.GenerateImpulse();
    }
}
