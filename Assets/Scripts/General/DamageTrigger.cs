using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageTrigger : MonoBehaviour
{
    private IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponentInParent<IDamageable>();
    }
    
    public void TakeDamage(int damage)
    {
        damageable.TakeDamage(damage);
    }
    
    public void AddForce(Vector2 direction, float force)
    {
        damageable.AddForce(direction, force);
    }
}
