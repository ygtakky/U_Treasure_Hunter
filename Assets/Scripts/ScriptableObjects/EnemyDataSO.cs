using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public int maxHealth;
    public Sprite sprite;
}
