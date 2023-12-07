using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDataSO", menuName = "Data/Scene Data")]
public class SceneDataSO : ScriptableObject
{
    public SceneAsset scene;
    public AudioClip music;
}
