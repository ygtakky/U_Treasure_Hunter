using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HealthBarUI : MonoBehaviour
{
    [Header("Listening Events")]
    [SerializeField] private VoidEventChannelSO playerHealthChangedChannel;
    
    [Header("Configuration")]
    [SerializeField] private HealthDataSO playerHealthData;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private RectTransform healthBarRectTransform;

    private void Start()
    {
        healthBarImage.fillAmount = 1f;
    }

    private void OnEnable()
    {
        playerHealthChangedChannel.OnEventRaised += PlayerHealthChangedChannel_OnEventRaised;
    }
    private void OnDisable()
    {
        playerHealthChangedChannel.OnEventRaised -= PlayerHealthChangedChannel_OnEventRaised;
    }
    
    private void PlayerHealthChangedChannel_OnEventRaised(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateHealthBar());
    }
    
    private IEnumerator AnimateHealthBar()
    {
        float from = healthBarImage.fillAmount;
        float to = (float)playerHealthData.CurrentHealth / playerHealthData.MaxHealth;
        float t = 0f;
        
        // Shake the health bar if the player is damaged
        Vector2 originalPosition = healthBarRectTransform.anchoredPosition;
        
        while (t < 1f)
        {
            t += Time.deltaTime;
            if (from > to)
            {
                Vector2 shakeAmount = Random.insideUnitCircle * 3f;
                healthBarRectTransform.anchoredPosition = originalPosition + shakeAmount;
            }
            else
            {
                healthBarRectTransform.anchoredPosition = originalPosition;
            }
            healthBarImage.fillAmount = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }
}
