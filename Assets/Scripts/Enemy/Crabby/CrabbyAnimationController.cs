using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrabbyAnimationController : MonoBehaviour
{
    [SerializeField] private CrabbyController controller;
    
    private Animator animator;
    private string currentState;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        controller.OnMove += Controller_OnMove;
        controller.OnMoveStop += Controller_OnMoveStop;
        controller.OnAttack += Controller_OnAttack;
        controller.OnHit += Controller_OnHit;
    }

    private void OnDisable()
    {
        controller.OnMove -= Controller_OnMove;
        controller.OnMoveStop -= Controller_OnMoveStop;
        controller.OnAttack -= Controller_OnAttack;
        controller.OnHit -= Controller_OnHit;
    }
    
    private void ChangeState(string state)
    {
        if (currentState == state)
        {
            return;
        }
        
        animator.Play(state);
        
        currentState = state;
    }
    
    private void Controller_OnMove(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(CrabbyAnimationStates.RUN);
    }
    
    private void Controller_OnMoveStop(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(CrabbyAnimationStates.IDLE);
    }
    
    private void Controller_OnAttack(object sender, EventArgs e)
    {
        ChangeState(CrabbyAnimationStates.ATTACK);
    }
    
    private void Controller_OnHit(object sender, EventArgs e)
    {
        ChangeState(CrabbyAnimationStates.HIT);
    }
    
    private bool IsAnimationPlaying(string state)
    {
        return currentState == state && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }
    
    private bool CanChangeAnimationState()
    {
        bool isAttacking = IsAnimationPlaying(CrabbyAnimationStates.ATTACK);
        bool isHit = IsAnimationPlaying(CrabbyAnimationStates.HIT);
        
        return !isAttacking && !isHit;
    }
    
    private void OnAttackAnimationEnd()
    {
        controller.SetIsAttacking(false);
        
        ChangeState(CrabbyAnimationStates.IDLE);
    }
    
    private void OnHitAnimationEnd()
    {
        controller.SetIsAttacking(false);
        
        ChangeState(CrabbyAnimationStates.IDLE);
    }
}

public static class CrabbyAnimationStates
{
    public static readonly string IDLE = "Idle";
    public static readonly string RUN = "Run";
    public static readonly string ATTACK = "Attack";
    public static readonly string HIT = "Hit";
}