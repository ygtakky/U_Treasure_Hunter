using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public int maxHealth;
    public float movementSpeed;
    public float jumpForce;
    public float maxFallSpeed;
    public float attackCooldown;
    public float attackRadius;
    public float slideSpeed;
}
