using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float movementSpeed;
    public float jumpForce;
    public float maxFallSpeed;
    public float attackCooldown;
}
