using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Broadcasting Events")]
    [SerializeField] private IntEventChannelSO scoreChannel;
    [SerializeField] private AudioEventChannelSO sfxChannel;
    
    public event EventHandler CollectiblePickedUp;
    
    [Header("Configuration")]
    [SerializeField] private CollectibleDataSO settings;
    [SerializeField] private AudioClip collectSFX;
    
    private bool isCollected;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            if (isCollected) return;
            OnPickup();
        }
    }

    private void OnPickup()
    {
        isCollected = true;
        scoreChannel.RaiseEvent(this, new IntEventArgs(settings.scoreValue));
        sfxChannel.RaiseEvent(this, new AudioEventArgs(collectSFX));
        CollectiblePickedUp?.Invoke(this, EventArgs.Empty);
    }
    
    public void DestroyCollectible()
    {
        Destroy(gameObject);
    }
}
