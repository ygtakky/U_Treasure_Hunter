using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Broadcasting Channels")]
    [SerializeField] private IntEventChannelSO gameWonChannel;
    
    [Header("Listening Channels")]
    [SerializeField] private IntEventChannelSO collectiblePickupChannel;
    [SerializeField] private IntEventChannelSO endingCollectiblePickupChannel;

    private int score = 0;

    private void OnEnable()
    {
        collectiblePickupChannel.OnEventRaised += CollectiblePickupChannel_OnEventRaised;
        endingCollectiblePickupChannel.OnEventRaised += EndingCollectiblePickupChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        collectiblePickupChannel.OnEventRaised -= CollectiblePickupChannel_OnEventRaised;
        endingCollectiblePickupChannel.OnEventRaised -= EndingCollectiblePickupChannel_OnEventRaised;
    }

    private void CollectiblePickupChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        score += e.Value;
    }
    
    private void EndingCollectiblePickupChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        score += e.Value;
        gameWonChannel.RaiseEvent(this, new IntEventArgs(score));
    }
}
