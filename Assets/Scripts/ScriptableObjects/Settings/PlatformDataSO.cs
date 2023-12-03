using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlatformData", menuName = "Data/Platform Data", order = 1)]
public class PlatformDataSO : ScriptableObject
{
    public PlatformTypes platformType;
    public float speed;
    public float waitTime;
}
