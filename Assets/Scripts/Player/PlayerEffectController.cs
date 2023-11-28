using System;
using Cinemachine;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private Animator attackEffectAnimator;

    private void OnEnable()
    {
        playerController.OnAttack += PlayerController_OnAttack;
    }

    private void OnDisable()
    {
        playerController.OnAttack -= PlayerController_OnAttack;
    }

    private void PlayerController_OnAttack(object sender, EventArgs e)
    {
        attackEffectAnimator.gameObject.SetActive(true);
    }
}
