using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO initialSceneData;
    [SerializeField] private SceneEventChannelSO sceneEventChannel;
    
    private IEnumerator Start()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Managers", LoadSceneMode.Additive);
        
        while (!operation.isDone)
        {
            yield return null;
        }
        
        sceneEventChannel.RaiseEvent(this, new SceneEventArgs(initialSceneData));
        
        yield return null;
    }
}
