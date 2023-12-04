using UnityEngine;

[CreateAssetMenu(fileName = "TippingPlatformData", menuName = "Data/Tipping Platform Data")]
public class TippingPlatformDataSO : ScriptableObject
{
    public float rotatingForce;
    public float resetRotationSpeed;
    public float resetRotationDelaySeconds;
}
