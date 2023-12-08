using System;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockBackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Damage(other);
        }
    }
    
    private void Damage(Collider2D other)
    {
        Vector2 direction = (other.transform.position - transform.position).normalized;
        DamageTrigger damageTrigger = other.GetComponent<DamageTrigger>();
        damageTrigger.TakeDamage(damage);
        damageTrigger.AddForce(direction, knockBackForce);
    }
}
