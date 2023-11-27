using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Events
    
    public event EventHandler OnMove;
    public event EventHandler OnStopMove;
    public event EventHandler OnJump;
    public event EventHandler OnFall;
    public event EventHandler OnLand;
    
    public event EventHandler OnAttack;
    
    #endregion
    
    public bool IsGrounded { get; private set; }
    
    [SerializeField] private PlayerData settings;
    
    [Header("Grounding Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isJumpPressed;
    private bool wasJumping;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool canAttack = true;
    private float attackTimer;
    
    #region Unity Events
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInputs();
        UpdateAttackTimer();
    }
        

    private void FixedUpdate()
    {
        CheckGrounded();
        Jump();
        Move();
        Attack();
    }
    
    #endregion
    
    #region Movement Methods
    
    private void Move()
    {
        Vector2 currentVelocity = rb.velocity;
        Vector2 newVelocity = currentVelocity;
        
        newVelocity.x = horizontalInput * settings.movementSpeed * Time.fixedDeltaTime;

        rb.velocity = Vector2.Lerp(currentVelocity, newVelocity, 10.0f);
        
        if (rb.velocity.y < settings.maxFallSpeed)
        {
            float clampedYSpeed = Mathf.Clamp(rb.velocity.y, -settings.maxFallSpeed, settings.maxFallSpeed);
            rb.velocity = new Vector2(rb.velocity.x, clampedYSpeed);
        }
        
        if (horizontalInput != 0.0f)
        {
            FlipSprite(horizontalInput);
        }

        CheckGravityScale();
        
        InvokeMovementEvents();
    }
    
    private void Jump()
    {
        if (isJumpPressed && IsGrounded)
        {
            rb.AddForce(Vector2.up * settings.jumpForce, ForceMode2D.Impulse);
            OnJump?.Invoke(this, EventArgs.Empty);
        }
        
        isJumpPressed = false;
    }
    
    #endregion
    
    #region Helper Methods
    
    private void Attack()
    {
        if (isAttackPressed && !isAttacking && canAttack)
        {
            canAttack = false;
            OnAttack?.Invoke(this, EventArgs.Empty);
        }
        
        isAttackPressed = false;
    }
    
    private void UpdateAttackTimer()
    {
        if (canAttack) return;
        
        attackTimer += Time.deltaTime;
        
        if (!(attackTimer >= settings.attackCooldown)) return;
        
        canAttack = true;
        attackTimer = 0.0f;
    }
    
    public void OnAttackCompleted()
    {
        isAttacking = false;
    }
    
    private void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            isAttackPressed = true;
        }
    }

    private void InvokeMovementEvents()
    {
        if (horizontalInput != 0.0f && IsGrounded)
        {
            OnMove?.Invoke(this, EventArgs.Empty);
        }
        else if (horizontalInput == 0.0f && IsGrounded)
        {
            OnStopMove?.Invoke(this, EventArgs.Empty);
        }
        
        if (IsFalling())
        {
            if (!wasJumping)
            {
                wasJumping = true;
            }
            
            OnFall?.Invoke(this, EventArgs.Empty);
        }
        
        if (IsGrounded && wasJumping)
        {
            wasJumping = false;
            OnLand?.Invoke(this, EventArgs.Empty);
        }
        
    }
    
    private void CheckGravityScale()
    {
        if (IsJumping())
        {
            rb.gravityScale = 1.0f;
        }
        else if (IsFalling())
        {
            rb.gravityScale = 2.0f;
        }
    }
    
    private void FlipSprite(float direction)
    {
        transform.localScale = new Vector3(direction, 1.0f, 1.0f);
    }
    
    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    
    private bool IsJumping()
    {
        return rb.velocity.y > 0.0f && !IsGrounded;
    }
    
    private bool IsFalling()
    {
        return rb.velocity.y < 0.0f && !IsGrounded;
    }
    
    #endregion
}
