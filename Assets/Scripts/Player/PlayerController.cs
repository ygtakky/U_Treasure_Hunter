using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IDamageable
{
    public bool IsGrounded { get; private set; }
    public Vector2 PlatformSpeed { get; set; }
    
    [SerializeField] private PlayerDataSO settings;
    
    [Header("Broadcasting on")]
    [SerializeField] private VoidEventChannelSO playerMoveChannel;
    [SerializeField] private VoidEventChannelSO playerStopMoveChannel;
    [SerializeField] private VoidEventChannelSO playerJumpChannel;
    [SerializeField] private VoidEventChannelSO playerFallChannel;
    [SerializeField] private VoidEventChannelSO playerLandChannel;
    [SerializeField] private VoidEventChannelSO playerAttackChannel;
    [SerializeField] private VoidEventChannelSO playerHealthChangedChannel;
    [SerializeField] private VoidEventChannelSO playerHitChannel;
    [SerializeField] private VoidEventChannelSO playerDeathChannel;
    [SerializeField] private AudioEventChannelSO sfxAudioEventChannel;
    [SerializeField] private CameraShakeEventChannelSO cameraShakeEventChannel;
    
    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO playerAttackCompletedChannel;
    [SerializeField] private VoidEventChannelSO attackButtonClickedChannel;
    [SerializeField] private VoidEventChannelSO jumpButtonClickedChannel;
    
    [Header("Grounding Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask attackLayer;
    
    [Header("Health Data")]
    [SerializeField] private HealthDataSO healthData;
    
    [Header("Mobile Controls")]
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private bool isDesktopControlsEnabled;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip landSfx;
    [SerializeField] private AudioClip hitSfx;
    
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isJumpPressed;
    private bool wasJumping;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool canAttack = true;
    private float attackTimer;
    private bool isDead;
    
    #region Unity Events
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (joystick == null)
        {
            joystick = FindObjectOfType<FixedJoystick>();
        }
    }

    private void Start()
    {
        healthData.SetMaxHealth(settings.maxHealth);
        healthData.ResetHealth();
        
        playerHealthChangedChannel.RaiseEvent(this);
    }

    private void OnEnable()
    {
        healthData.SetCurrentHealth(settings.maxHealth);
        
        playerAttackCompletedChannel.OnEventRaised += OnAttackCompleted;
        attackButtonClickedChannel.OnEventRaised += AttackButtonClickedChannel_OnAttackButtonClicked;
        jumpButtonClickedChannel.OnEventRaised += JumpButtonClickedChannel_OnJumpButtonClicked;
    }

    private void OnDisable()
    {
        playerAttackCompletedChannel.OnEventRaised -= OnAttackCompleted;
        attackButtonClickedChannel.OnEventRaised -= AttackButtonClickedChannel_OnAttackButtonClicked;
        jumpButtonClickedChannel.OnEventRaised -= JumpButtonClickedChannel_OnJumpButtonClicked;
    }

    private void Update()
    {
        if (isDead) return;
        
        GetInputs();
        UpdateAttackTimer();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        
        CheckGrounded();
        Jump();
        Move();
        Slide();
        Attack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, settings.attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + (horizontalInput * Vector3.right * 0.35f));
    }

    #endregion
    
    #region Movement Methods
    
    private void Move()
    {
        Vector2 currentVelocity = rb.velocity;
        Vector2 newVelocity = currentVelocity;
        
        newVelocity.x = horizontalInput * settings.movementSpeed * Time.fixedDeltaTime;
        
        if (newVelocity.x == 0.0f)
        {
            newVelocity += new Vector2(PlatformSpeed.x, 0.0f);
        }

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
            playerJumpChannel.RaiseEvent(this);
            sfxAudioEventChannel.RaiseEvent(this, new AudioEventArgs(jumpSfx));
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
            playerAttackChannel.RaiseEvent(this);
            sfxAudioEventChannel.RaiseEvent(this, new AudioEventArgs(attackSfx));
            
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, settings.attackRadius, attackLayer);
        
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(1);
                }
            }
        }
        
        isAttackPressed = false;
    }
    
    private void Slide()
    {
        if (IsGrounded || IsJumping()) return;
        
        float inputDirection = horizontalInput;
        
        RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 0.5f), CapsuleDirection2D.Vertical, 0.0f, inputDirection * Vector2.right, 0.25f, groundLayer);
        
        if (hit.collider != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    
    private void UpdateAttackTimer()
    {
        if (canAttack) return;
        
        attackTimer += Time.deltaTime;
        
        if (!(attackTimer >= settings.attackCooldown)) return;
        
        canAttack = true;
        attackTimer = 0.0f;
    }
    
    private void OnAttackCompleted(object sender, EventArgs e)
    {
        isAttacking = false;
    }
    
    private void AttackButtonClickedChannel_OnAttackButtonClicked(object sender, EventArgs e)
    {
        isAttackPressed = true;
    }
    
    private void JumpButtonClickedChannel_OnJumpButtonClicked(object sender, EventArgs e)
    {
        isJumpPressed = true;
    }
    
    private void GetInputs()
    {
        horizontalInput = joystick.Horizontal;
        if (horizontalInput < 0.0f)
        {
            horizontalInput = -1.0f;
        }
        else if (horizontalInput > 0.0f)
        {
            horizontalInput = 1.0f;
        }
        
        if (isDesktopControlsEnabled)
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
        
    }

    private void InvokeMovementEvents()
    {
        if (horizontalInput != 0.0f && IsGrounded)
        {
            playerMoveChannel.RaiseEvent(this);
        }
        else if (horizontalInput == 0.0f && IsGrounded)
        {
            playerStopMoveChannel.RaiseEvent(this);
        }
        
        if (IsFalling())
        {
            if (!wasJumping)
            {
                wasJumping = true;
            }
            
            playerFallChannel.RaiseEvent(this);
        }
        
        if (IsGrounded && wasJumping)
        {
            wasJumping = false;
            playerLandChannel.RaiseEvent(this);
            sfxAudioEventChannel.RaiseEvent(this, new AudioEventArgs(landSfx));
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

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        playerHitChannel.RaiseEvent(this);
        sfxAudioEventChannel.RaiseEvent(this, new AudioEventArgs(hitSfx));
        cameraShakeEventChannel.RaiseEvent(this, new CameraShakeEventArgs(0.1f, 0.1f, 0.1f));
        
        healthData.TakeDamage(damage);

        if (healthData.CurrentHealth <= 0)
        {
            playerDeathChannel.RaiseEvent(this);
            isDead = true;
        }
        
        playerHealthChangedChannel.RaiseEvent(this);
    }
    
    public void AddForce(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
