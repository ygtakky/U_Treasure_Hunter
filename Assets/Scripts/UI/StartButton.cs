using UnityEngine;

public class StartButton : BaseButton
{
    [SerializeField] private SceneEventChannelSO sceneChangedEventChannel;
    [SerializeField] private SceneDataSO sceneToLoad;
    
    protected override void Start()
    {
        base.Start();
        
        AddOnClickListener(LoadGame);
    }

    private void LoadGame()
    {
        sceneChangedEventChannel.RaiseEvent(this, new SceneEventArgs(sceneToLoad));
    }
}
