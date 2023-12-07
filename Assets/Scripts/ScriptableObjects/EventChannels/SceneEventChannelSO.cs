using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event Channels/Scene Event Channel")]
public class SceneEventChannelSO : GenericEventChannelSO<SceneEventArgs>
{
}

public class SceneEventArgs : EventArgs
{
    public SceneDataSO SceneData;
    
    public SceneEventArgs(SceneDataSO sceneData)
    {
        SceneData = sceneData;
    }
}
