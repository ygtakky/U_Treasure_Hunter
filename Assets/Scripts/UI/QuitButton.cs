using UnityEngine;

public class QuitButton : BaseButton
{
    [Header("Broadcasting on")]
    [SerializeField] private SceneEventChannelSO sceneChangeChannel;

    [Header("Configuration")] [SerializeField]
    private SceneDataSO mainMenuSceneData;
    
    protected override void Start()
    {
        base.Start();
        
        AddOnClickListener(OpenMainMenu);
    }
    
    private void OpenMainMenu()
    {
        sceneChangeChannel.RaiseEvent(this, new SceneEventArgs(mainMenuSceneData));
    }
}
