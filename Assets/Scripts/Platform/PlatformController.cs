using System;
using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private PlatformDataSO settings;
    [SerializeField] private Transform[] patrolPoints;
    
    private Rigidbody2D rb2D;
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        if (settings.platformType == PlatformTypes.Patrol)
        {
            StartCoroutine(Patrol());
        }
    }

    private IEnumerator Patrol()
    {
        // TODO: Add the patrol code
        yield return null;
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // TODO: ADD the gizmos code
    }
    #endif
}