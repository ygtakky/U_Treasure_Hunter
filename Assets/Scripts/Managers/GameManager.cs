using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Broadcasting Channels")]
    [SerializeField] private IntEventChannelSO gameWonChannel;
    [SerializeField] private IntEventChannelSO gameLostChannel;
    
    [Header("Listening Channels")]
    [SerializeField] private IntEventChannelSO scoreChannel;
    [SerializeField] private IntEventChannelSO endingCollectiblePickupChannel;
    [SerializeField] private VoidEventChannelSO playerDeathChannel;

    private int score = 0;

    private void OnEnable()
    {
        scoreChannel.OnEventRaised += ScoreChannel_OnEventRaised;
        endingCollectiblePickupChannel.OnEventRaised += EndingCollectiblePickupChannel_OnEventRaised;
        playerDeathChannel.OnEventRaised += PlayerDeathChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        scoreChannel.OnEventRaised -= ScoreChannel_OnEventRaised;
        endingCollectiblePickupChannel.OnEventRaised -= EndingCollectiblePickupChannel_OnEventRaised;
        playerDeathChannel.OnEventRaised -= PlayerDeathChannel_OnEventRaised;
    }

    private void ScoreChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        score += e.Value;
    }
    
    private void EndingCollectiblePickupChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        score += e.Value;
        gameWonChannel.RaiseEvent(this, new IntEventArgs(score));
    }
    
    private void PlayerDeathChannel_OnEventRaised(object sender, EventArgs e)
    {
        gameLostChannel.RaiseEvent(this, new IntEventArgs(score));
    }
}
