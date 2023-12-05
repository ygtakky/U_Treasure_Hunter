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
    }
    
    private void OnDisable()
    {
        controller.OnMove -= Controller_OnMove;
        controller.OnMoveStop -= Controller_OnMoveStop;
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
        ChangeState(CrabbyAnimationStates.RUN);
    }
    
    private void Controller_OnMoveStop(object sender, EventArgs e)
    {
        ChangeState(CrabbyAnimationStates.IDLE);
    }
}

public static class CrabbyAnimationStates
{
    public static readonly string IDLE = "Idle";
    public static readonly string RUN = "Run";
}