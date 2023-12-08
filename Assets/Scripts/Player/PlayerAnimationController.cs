using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Broadcasting Events")]
    [SerializeField] private VoidEventChannelSO playerAttackCompletedChannel;
    
    [Header("Listening Events")]
    [SerializeField] private VoidEventChannelSO playerMoveChannel;
    [SerializeField] private VoidEventChannelSO playerStopMoveChannel;
    [SerializeField] private VoidEventChannelSO playerJumpChannel;
    [SerializeField] private VoidEventChannelSO playerFallChannel;
    [SerializeField] private VoidEventChannelSO playerLandChannel;
    [SerializeField] private VoidEventChannelSO playerAttackChannel;
    [SerializeField] private VoidEventChannelSO playerHitChannel;
    [SerializeField] private VoidEventChannelSO playerDeathChannel;
    
    private Animator animator;
    private string currentState;
    
    #region Unity Events
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerMoveChannel.OnEventRaised += PlayerMoveChannel_OnMove;
        playerStopMoveChannel.OnEventRaised += PlayerStopMoveChannel_OnStopMove;
        playerJumpChannel.OnEventRaised += PlayerJumpChannel_OnJump;
        playerFallChannel.OnEventRaised += PlayerFallChannel_OnFall;
        playerLandChannel.OnEventRaised += PlayerLandChannel_OnLand;
        playerAttackChannel.OnEventRaised += PlayerAttackChannel_OnAttack;
        playerHitChannel.OnEventRaised += PlayerHitChannel_OnHit;
        playerDeathChannel.OnEventRaised += PlayerDeathChannel_OnDeath;
    }

    private void OnDisable()
    {
        playerMoveChannel.OnEventRaised -= PlayerMoveChannel_OnMove;
        playerStopMoveChannel.OnEventRaised -= PlayerStopMoveChannel_OnStopMove;
        playerJumpChannel.OnEventRaised -= PlayerJumpChannel_OnJump;
        playerFallChannel.OnEventRaised -= PlayerFallChannel_OnFall;
        playerLandChannel.OnEventRaised -= PlayerLandChannel_OnLand;
        playerAttackChannel.OnEventRaised -= PlayerAttackChannel_OnAttack;
        playerHitChannel.OnEventRaised -= PlayerHitChannel_OnHit;
        playerDeathChannel.OnEventRaised -= PlayerDeathChannel_OnDeath;
    }
    
    #endregion
    
    #region Event Handlers
    
    private void PlayerMoveChannel_OnMove(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(PlayerAnimationStates.RUN);
    }

    private void PlayerStopMoveChannel_OnStopMove(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(PlayerAnimationStates.IDLE);
    }
    
    private void PlayerJumpChannel_OnJump(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.JUMP);
    }
    
    private void PlayerFallChannel_OnFall(object sender, EventArgs e)
    {
        if (!CanChangeAnimationState())
        {
            return;
        }
        
        ChangeState(PlayerAnimationStates.FALL);
    }
    
    private void PlayerLandChannel_OnLand(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.LAND);
    }
    
    private void PlayerAttackChannel_OnAttack(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.ATTACK);
    }
    
    private void PlayerHitChannel_OnHit(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.HIT);
    }
    
    private void PlayerDeathChannel_OnDeath(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.DEATH);
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
        bool isLanding = IsAnimationPlaying(PlayerAnimationStates.LAND);
        bool isAttacking = IsAnimationPlaying(PlayerAnimationStates.ATTACK);
        bool isJumping = IsAnimationPlaying(PlayerAnimationStates.JUMP);
        bool isHit = IsAnimationPlaying(PlayerAnimationStates.HIT);
        
        return !isLanding && !isAttacking && !isJumping && !isHit;
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
        playerAttackCompletedChannel.RaiseEvent(this);
        
        ChangeState(PlayerAnimationStates.IDLE);
    }
    
    private void OnHitAnimationCompleted()
    {
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
    public const string HIT = "Player_Hit";
    public const string DEATH = "Player_Death";
}