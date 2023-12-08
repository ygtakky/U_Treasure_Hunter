using System;
using TMPro;
using UnityEngine;

public class GameOverUI : BaseUI
{
    [Header("Listening to")]
    [SerializeField] private IntEventChannelSO gameWonChannel;
    [SerializeField] private IntEventChannelSO gameLostChannel;
    
    [Header("Configuration")]
    [SerializeField] private TMP_Text leftTitleText;
    [SerializeField] private TMP_Text rightTitleText;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        gameWonChannel.OnEventRaised += GameWonChannel_OnEventRaised;
        gameLostChannel.OnEventRaised += GameLostChannel_OnEventRaised;
        
        Hide();
    }
    
    private void OnDestroy()
    {
        gameWonChannel.OnEventRaised -= GameWonChannel_OnEventRaised;
        gameLostChannel.OnEventRaised -= GameLostChannel_OnEventRaised;
    }
    
    private void GameWonChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        leftTitleText.text = "You";
        rightTitleText.text = "Won!";
        scoreText.text = $"Score : {e.Value}";
        Show();
    }
    
    private void GameLostChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        leftTitleText.text = "You";
        rightTitleText.text = "Lost!";
        scoreText.text = $"Score : {e.Value}";
        Show();
    }
}
