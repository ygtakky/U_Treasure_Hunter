using UnityEngine;
using UnityEngine.UI;

public class ButtonsUI : MonoBehaviour
{
    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO attackButtonClickedChannel;
    [SerializeField] private VoidEventChannelSO jumpButtonClickedChannel;
    
    [Header("Configuration")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button jumpButton;

    private void Awake()
    {
        attackButton.onClick.AddListener(OnAttackButtonClicked);
        jumpButton.onClick.AddListener(OnJumpButtonClicked);
    }
    
    private void OnAttackButtonClicked()
    {
        attackButtonClickedChannel.RaiseEvent(this);
    }
    
    private void OnJumpButtonClicked()
    {
        jumpButtonClickedChannel.RaiseEvent(this);
    }
}
