using System;
using Cinemachine;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO playerAttackChannel;
    [SerializeField] private Animator attackEffectAnimator;

    private void OnEnable()
    {
        playerAttackChannel.OnEventRaised += PlayerController_OnAttack;
    }

    private void OnDisable()
    {
        playerAttackChannel.OnEventRaised -= PlayerController_OnAttack;
    }

    private void PlayerController_OnAttack(object sender, EventArgs e)
    {
        attackEffectAnimator.gameObject.SetActive(true);
    }
}
