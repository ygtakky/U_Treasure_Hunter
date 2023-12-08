using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("Listening Events")]
    [SerializeField] private IntEventChannelSO scoreChannel;
    [SerializeField] private IntEventChannelSO endingCollectiblePickupChannel;
    
    [Header("Configuration")]
    [SerializeField] private TMP_Text scoreText;
    
    private int score;
    
    private void Start()
    {
        UpdateText();
    }
    
    private void OnEnable()
    {
        scoreChannel.OnEventRaised += ScoreChannel_OnEventRaised;
        endingCollectiblePickupChannel.OnEventRaised += ScoreChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        scoreChannel.OnEventRaised -= ScoreChannel_OnEventRaised;
        endingCollectiblePickupChannel.OnEventRaised -= ScoreChannel_OnEventRaised;
    }
    
    private void ScoreChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        score += e.Value;
        UpdateText();
    }
    
    private void UpdateText()
    {
        scoreText.text = $"{score}";
    }
}
