using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    private Animator animator;
    private string currentState;
    
    #region Unity Events
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerController.OnMove += PlayerController_OnMove;
        playerController.OnStopMove += PlayerController_OnStopMove;
        playerController.OnJump += PlayerController_OnJump;
        playerController.OnFall += PlayerController_OnFall;
        playerController.OnLand += PlayerController_OnLand;
        playerController.OnAttack += PlayerController_OnAttack;
    }

    private void OnDisable()
    {
        playerController.OnMove -= PlayerController_OnMove;
        playerController.OnStopMove -= PlayerController_OnStopMove;
        playerController.OnJump -= PlayerController_OnJump;
        playerController.OnFall -= PlayerController_OnFall;
        playerController.OnLand -= PlayerController_OnLand;
        playerController.OnAttack -= PlayerController_OnAttack;
    }
    
    #endregion
    
    #region Event Handlers
    
    private void PlayerController_OnMove(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(PlayerAnimationStates.RUN);
    }

    private void PlayerController_OnStopMove(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(PlayerAnimationStates.IDLE);
    }
    
    private void PlayerController_OnJump(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.JUMP);
    }
    
    private void PlayerController_OnFall(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(PlayerAnimationStates.FALL);
    }
    
    private void PlayerController_OnLand(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.LAND);
    }
    
    private void PlayerController_OnAttack(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.ATTACK);
    }
    
    #endregion
    
    private void ChangeState(string state)
    {
        if (currentState == state)
        {
            return;
        }
        
        animator.Play(state);
        
        currentState = state;
    }
    
    private bool CanChangeAnimationState()
    {
        return !IsAnimationPlaying(PlayerAnimationStates.LAND) && !IsAnimationPlaying(PlayerAnimationStates.ATTACK);
    }
    
    private bool IsAnimationPlaying(string state)
    {
        return currentState == state && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
    }
    
    private void OnLandAnimationCompleted()
    {
        ChangeState(PlayerAnimationStates.IDLE);
    }
    
    private void OnAttackAnimationCompleted()
    {
        playerController.OnAttackCompleted();
        
        ChangeState(PlayerAnimationStates.IDLE);
    }
}

public static class PlayerAnimationStates
{
    public const string IDLE = "Player_Idle";
    public const string RUN = "Player_Run";
    public const string JUMP = "Player_Jump";
    public const string FALL = "Player_Fall";
    public const string LAND = "Player_Land";
    public const string ATTACK = "Player_Attack";
}