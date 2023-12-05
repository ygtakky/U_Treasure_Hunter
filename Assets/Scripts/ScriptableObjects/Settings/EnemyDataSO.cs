using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public int maxHealth;
    public float moveSpeed;
    public float moveAcceleration;
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;
    public float aggroRadius;
    public Sprite sprite;
}
