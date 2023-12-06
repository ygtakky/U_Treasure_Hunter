using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Broadcasting Events")]
    [SerializeField] private IntEventChannelSO collectiblePickupChannel;
    
    public event EventHandler CollectiblePickedUp;
    
    [Header("Configuration")]
    [SerializeField] private CollectibleDataSO settings;
    
    private bool isCollected;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isCollected) return;
            OnPickup();
        }
    }

    private void OnPickup()
    {
        isCollected = true;
        collectiblePickupChannel.RaiseEvent(this, new IntEventArgs(settings.scoreValue));
        CollectiblePickedUp?.Invoke(this, EventArgs.Empty);
    }
    
    public void DestroyCollectible()
    {
        Destroy(gameObject);
    }
}
