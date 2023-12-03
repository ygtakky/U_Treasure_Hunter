using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyDataSO settings;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = settings.maxHealth;
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        spriteRenderer.sprite = settings.sprite;
    }
    #endif

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            // TODO: Add enemy death logic
        }
        
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");
    }
}
