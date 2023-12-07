using UnityEngine;

public class InitialLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO initialSceneData;
    [SerializeField] private SceneEventChannelSO sceneEventChannel;
    
    private void Start()
    {
        sceneEventChannel.RaiseEvent(this, new SceneEventArgs(initialSceneData));
    }
}
