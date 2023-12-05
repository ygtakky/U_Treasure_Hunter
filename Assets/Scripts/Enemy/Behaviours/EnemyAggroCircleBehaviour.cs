using System;
using System.Collections;
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
            StartCoroutine(DelayAggro());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(DelayAggro());
            
            aggroEntity.SetPlayerInRange(false);
        }
    }
    
    private IEnumerator DelayAggro()
    {
        yield return new WaitForSeconds(settings.aggroStartDelay);
        aggroEntity.SetPlayerInRange(true);

        yield return null;
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.aggroRadius);
    }
    #endif
}
