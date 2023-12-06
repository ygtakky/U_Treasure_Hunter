using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Data/Health Data")]
public class HealthDataSO : ScriptableObject
{
    [SerializeField][ReadOnly] private int maxHealth;
    [SerializeField][ReadOnly] private int currentHealth;
    
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
    }
    
    public void SetCurrentHealth(int value)
    {
        currentHealth = value;
    }
    
    public void TakeDamage(int value)
    {
        currentHealth -= value;
    }
    
    public void RestoreHealth(int value)
    {
        currentHealth += value;
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

}
