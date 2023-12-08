using UnityEngine;

[CreateAssetMenu(fileName = "SceneDataSO", menuName = "Data/Scene Data")]
public class SceneDataSO : ScriptableObject
{
    public string sceneName;
    public AudioClip music;
}
