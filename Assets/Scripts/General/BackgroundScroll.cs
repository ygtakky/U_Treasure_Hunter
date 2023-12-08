using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private List<Transform> backgroundList;
    [SerializeField] private float scrollSpeed;
    
    private void Start()
    {
        backgroundList = new List<Transform>();
        foreach (Transform child in transform)
        {
            backgroundList.Add(child);
        }

        for (int i = 0; i < backgroundList.Count; i++)
        {
            backgroundList[i].position = new Vector3(i * 28, backgroundList[i].position.y, backgroundList[i].position.z);
        }
    }
    
    private void Update()
    {
        foreach (var background in backgroundList)
        {
            background.position += Vector3.left * (scrollSpeed * Time.deltaTime);
        }
        
        if (backgroundList.Count > 1 && backgroundList[0].position.x < -28)
        {
            backgroundList[0].position = new Vector3(backgroundList[1].position.x + 28, backgroundList[0].position.y, backgroundList[0].position.z);
            backgroundList.Add(backgroundList[0]);
            backgroundList.RemoveAt(0);
        }
    }
}
