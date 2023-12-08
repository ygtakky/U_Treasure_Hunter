using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    void AddForce(Vector2 direction, float force);
}
