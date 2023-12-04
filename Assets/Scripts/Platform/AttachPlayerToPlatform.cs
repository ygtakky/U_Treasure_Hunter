using System;
using UnityEngine;

public class AttachPlayerToPlatform : MonoBehaviour
{
    private Rigidbody2D platformRigidbody2D;
    private PlayerController playerController;
    
    private void Awake()
    {
        platformRigidbody2D = GetComponentInParent<Rigidbody2D>();
    }
    
    private void Update()
    {
        Vector2 currentPlatformSpeed = platformRigidbody2D.velocity;
        
        if (playerController == null || playerController.PlatformSpeed == currentPlatformSpeed)
        {
            return;
        }
        
        playerController.PlatformSpeed = currentPlatformSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            playerController.PlatformSpeed = platformRigidbody2D.velocity;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.PlatformSpeed = Vector2.zero;
            playerController = null;
        }
    }
}
