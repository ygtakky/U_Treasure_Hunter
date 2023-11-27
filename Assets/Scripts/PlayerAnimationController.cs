using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    private Animator animator;
    private string currentState;
    
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
    }

    private void OnDisable()
    {
        playerController.OnMove -= PlayerController_OnMove;
        playerController.OnStopMove -= PlayerController_OnStopMove;
        playerController.OnJump -= PlayerController_OnJump;
        playerController.OnFall -= PlayerController_OnFall;
        playerController.OnLand -= PlayerController_OnLand;
    }
    
    #region Event Handlers
    
    private void PlayerController_OnMove(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.RUN);
    }

    private void PlayerController_OnStopMove(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.IDLE);
    }
    
    private void PlayerController_OnJump(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.JUMP);
    }
    
    private void PlayerController_OnFall(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.FALL);
    }
    
    private void PlayerController_OnLand(object sender, EventArgs e)
    {
        ChangeState(PlayerAnimationStates.IDLE);
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
}

public class PlayerAnimationStates
{
    public const string IDLE = "Player_Idle";
    public const string RUN = "Player_Run";
    public const string JUMP = "Player_Jump";
    public const string FALL = "Player_Fall";
}