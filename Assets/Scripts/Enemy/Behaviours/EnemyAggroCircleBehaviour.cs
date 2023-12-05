using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyAggroCircleBehaviour : MonoBehaviour
{
    [SerializeField] private EnemyDataSO settings;
    
    private IAggroable aggroEntity;
    private CircleCollider2D circleCollider2D;
    
    private void Awake()
    {
        aggroEntity = GetComponentInParent<IAggroable>();
        
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = settings.aggroRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            aggroEntity.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            aggroEntity.SetPlayerInRange(false);
        }
    }
}
