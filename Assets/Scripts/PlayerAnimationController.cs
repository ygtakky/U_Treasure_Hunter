using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private Animator animator;
    
    private static readonly int HorizontalVelocity = Animator.StringToHash("HorizontalVelocity");
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        Vector2 playerVelocity = playerController.GetVelocity().normalized;
        bool isFalling = playerController.IsFalling();
        bool isJumping = playerController.IsJumping();
        bool isMoving = playerVelocity.x != 0.0f;
        
        SetFloat(HorizontalVelocity, playerVelocity.x);
        SetFloat(VerticalVelocity, playerVelocity.y);
        SetBool(IsMoving, isMoving);
        SetBool(IsJumping, isJumping);
        SetBool(IsFalling, isFalling);
    }
    
    private void SetBool(int parameterHash, bool value)
    {
        animator.SetBool(parameterHash, value);
    }
    
    private void SetFloat(int parameterHash, float value)
    {
        animator.SetFloat(parameterHash, value);
    }
}
