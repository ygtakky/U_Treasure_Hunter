using System;
using System.Collections;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private MovingPlatformDataSO settings;
    [SerializeField] private Transform patrolPointsParent;
    [SerializeField] private Transform[] patrolPoints;
    
    private Rigidbody2D rb2D;
    private int currentPatrolPointIndex;
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        GetPatrolPoints();
        
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        if (patrolPoints.Length == 0) yield break;
        
        while (true)
        {
            Vector2 targetPosition = patrolPoints[currentPatrolPointIndex].position;
            Vector2 direction = (targetPosition - (Vector2) transform.position).normalized;
            Vector2 velocity = direction * settings.speed;
            
            rb2D.velocity = velocity;
            
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                rb2D.velocity = Vector2.zero;
                
                yield return new WaitForSeconds(settings.waitTime);
                
                GetNextPatrolPoint();
            }
            
            yield return null;
        }
    }

    private void GetNextPatrolPoint()
    {
        currentPatrolPointIndex++;
        
        if (currentPatrolPointIndex >= patrolPoints.Length)
        {
            currentPatrolPointIndex = 0;
        }
    }

    #if UNITY_EDITOR
    
    [ContextMenu("Get Patrol Points")]
    private void GetPatrolPoints()
    {
        if (patrolPointsParent == null && patrolPoints.Length == patrolPointsParent.childCount) return;
        
        patrolPoints = new Transform[patrolPointsParent.childCount];

        for (int i = 0; i < patrolPointsParent.childCount; i++)
        {
            patrolPoints[i] = patrolPointsParent.GetChild(i);
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints == null) return;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;
            
            Gizmos.color = i == 0 ? Color.red : Color.green;
            
            DrawPatrolPoint(i);
            DrawLineBetweenPatrolPoints(i);
        }
    }
    
    private void DrawPatrolPoint(int index)
    {
        Gizmos.DrawCube(patrolPoints[index].position, Vector3.one * 0.2f);
    }
    
    private void DrawLineBetweenPatrolPoints(int index)
    {
        if (index + 1 < patrolPoints.Length && patrolPoints[index + 1] != null)
        {
            Gizmos.DrawLine(patrolPoints[index].position, patrolPoints[index + 1].position);
        }
    }

    private void OnValidate()
    {
        GetPatrolPoints();
    }

#endif
}