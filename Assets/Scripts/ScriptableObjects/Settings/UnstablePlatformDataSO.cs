using UnityEngine;

[CreateAssetMenu(fileName = "UnstablePlatformData", menuName = "Data/Unstable Platform Data")]
public class UnstablePlatformDataSO : ScriptableObject
{
    public float shakeDuration;
    public float shakeMagnitude;
    public float respawnTime;
}
