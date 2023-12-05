using System;
using UnityEngine;

public class CollectibleAnimationController : MonoBehaviour
{
    [SerializeField] private Collectible collectible;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        collectible.CollectiblePickedUp += Collectible_OnPickup;
    }
    
    private void OnDisable()
    {
        collectible.CollectiblePickedUp -= Collectible_OnPickup;
    }
    
    private void Collectible_OnPickup(object sender, EventArgs e)
    {
        PlayCollectibleAnimation();
    }
    
    private void PlayCollectibleAnimation()
    {
        animator.Play("Collect");
    }
    
    public void DestroyCollectible()
    {
        collectible.DestroyCollectible();
    }
}
