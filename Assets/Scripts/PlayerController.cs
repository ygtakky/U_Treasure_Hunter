using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    
    [SerializeField] private PlayerData settings;
    
    [Header("Grounding Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isJumpButtonPressed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGrounded();
        GetInputs();
    }
        

    private void FixedUpdate()
    {
        Jump();
        Move();
    }
    
    private void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            isJumpButtonPressed = true;
        }
    }
    
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

        if (IsJumping())
        {
            rb.gravityScale = 1.0f;
        }
        else if (IsFalling())
        {
            rb.gravityScale = 2.0f;
        }
        
        if (horizontalInput != 0.0f)
        {
            transform.localScale = new Vector3(horizontalInput, 1.0f, 1.0f);
        }
    }
    
    private void Jump()
    {
        if (isJumpButtonPressed && IsGrounded)
        {
            rb.AddForce(Vector2.up * settings.jumpForce, ForceMode2D.Impulse);
        }
        
        isJumpButtonPressed = false;
    }
    
    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    
    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    
    public bool IsJumping()
    {
        return rb.velocity.y > 0.0f && !IsGrounded;
    }
    
    public bool IsFalling()
    {
        return rb.velocity.y < 0.0f && !IsGrounded;
    }
}
