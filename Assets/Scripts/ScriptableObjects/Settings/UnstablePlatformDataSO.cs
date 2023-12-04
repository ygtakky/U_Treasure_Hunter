using UnityEngine;

[CreateAssetMenu(fileName = "UnstablePlatformData", menuName = "Data/Unstable Platform Data", order = 1)]
public class UnstablePlatformDataSO : ScriptableObject
{
    public float shakeDuration;
    public float shakeMagnitude;
    public float respawnTime;
}
