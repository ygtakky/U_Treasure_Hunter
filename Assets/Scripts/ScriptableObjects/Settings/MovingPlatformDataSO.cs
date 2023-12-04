using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MovingPlatformData", menuName = "Data/Moving Platform Data", order = 1)]
public class MovingPlatformDataSO : ScriptableObject
{
    public float speed;
    public float waitTime;
}
