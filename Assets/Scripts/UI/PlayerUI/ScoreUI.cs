using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("Listening Events")]
    [SerializeField] private IntEventChannelSO collectiblePickupChannel;
    
    [Header("Configuration")]
    [SerializeField] private TMP_Text scoreText;
    
    private int score;
    
    private void Start()
    {
        UpdateText();
    }
    
    private void OnEnable()
    {
        collectiblePickupChannel.OnEventRaised += CollectiblePickupChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        collectiblePickupChannel.OnEventRaised -= CollectiblePickupChannel_OnEventRaised;
    }
    
    private void CollectiblePickupChannel_OnEventRaised(object sender, IntEventArgs e)
    {
        score += e.Value;
        UpdateText();
    }
    
    private void UpdateText()
    {
        scoreText.text = $"Score: {score}";
    }
}
