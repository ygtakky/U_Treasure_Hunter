using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
    public float jumpForce;
    public float maxFallSpeed;
    public float attackCooldown;
    public float attackRadius;
}
