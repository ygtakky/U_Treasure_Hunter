using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Broadcasting on")]
    [SerializeField] private AudioEventChannelSO musicEventChannel;
    
    [Header("Listening to")]
    [SerializeField] private SceneEventChannelSO sceneEventChannel;
    
    [Header("Debugging")]
    [SerializeField] private bool playDebugScene;
    [SerializeField] private SceneDataSO debugSceneData;

    private bool isLoadingScene;

    private void Start()
    {
        if (playDebugScene)
        {
            sceneEventChannel.RaiseEvent(this, new SceneEventArgs(debugSceneData));
        }
    }

    private void OnEnable()
    {
        sceneEventChannel.OnEventRaised += SceneEventChannel_OnEventRaised;
    }
    
    private void OnDisable()
    {
        sceneEventChannel.OnEventRaised -= SceneEventChannel_OnEventRaised;
    }
    
    private void SceneEventChannel_OnEventRaised(object sender, SceneEventArgs e)
    {
        if (isLoadingScene)
        {
            return;
        }
        
        StartCoroutine(LoadSceneAsync(e.SceneData));
    }
    
    private IEnumerator LoadSceneAsync(SceneDataSO sceneData)
    {
        isLoadingScene = true;
        
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);

        while (!unloadOperation.isDone)
        {
            yield return null;
        }
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        
        asyncOperation.completed += (obj) =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneData.sceneName));
        };

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                if (sceneData.music != null)
                {
                    musicEventChannel.RaiseEvent(this, new AudioEventArgs(sceneData.music));
                }
                
                isLoadingScene = false;
            }

            yield return null;
        }
    }
}
