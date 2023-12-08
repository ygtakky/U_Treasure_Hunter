using UnityEngine;

[CreateAssetMenu(fileName = "ButtonData", menuName = "Data/Button Settings")]
public class ButtonDataSO : ScriptableObject
{
    [Header("Broadcasting on")]
    [SerializeField] private AudioEventChannelSO sfxEventChannel;
    
    [Header("Configuration")]
    [SerializeField] private AudioClip buttonClickSFX;

    public AudioEventChannelSO SfxEventChannel => sfxEventChannel;
    public AudioClip ButtonClickSfx => buttonClickSFX;
}
