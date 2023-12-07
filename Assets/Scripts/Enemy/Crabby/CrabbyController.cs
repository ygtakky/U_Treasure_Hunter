using System;
using UnityEngine;

public class CrabbyController : MonoBehaviour, IDamageable, IMoveable, IAggroable
{
    public event EventHandler OnMove;
    public event EventHandler OnMoveStop;
    public event EventHandler OnAttack;
    public event EventHandler OnHit;
    public event EventHandler OnDeath;
    
    [Header("Configuration")]
    [SerializeField] private EnemyDataSO settings;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform attackPoint;
    [SerializeField][ReadOnly] private HealthDataSO healthData;
    
    private Rigidbody2D rb2D;
    private bool isPlayerInRange;
    private bool isAttacking;
    private bool canAttack = true;
    private PlayerController playerController;
    private float attackTimer;
    private bool isDead;
    
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
        
        healthData = ScriptableObject.CreateInstance<HealthDataSO>();
    }
    
    private void Update()
    {
        if (isDead)
        {
            return;
        }
        
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= settings.attackCooldown)
            {
                attackTimer = 0.0f;
                canAttack = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        
        if (isPlayerInRange)
        {
            if (GetSqrDistanceToPlayer() <= settings.attackRange * settings.attackRange)
            {
                StopMoving();
            }
            else
            {
                MoveTowards(playerController.transform.position);
            }
            
            if (canAttack)
            {
                Attack();
            }
        }
        else
        {
            StopMoving();
        }
    }

    private void OnEnable()
    {
        healthData.SetMaxHealth(settings.maxHealth);
        healthData.ResetHealth();
    }
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        spriteRenderer.sprite = settings.sprite;
    }
    
    private void OnDrawGizmos()
    {
        if (settings == null || attackPoint == null)
        {
            return;
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.attackRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, 0.1f);
    }
    #endif

    #region Movement 
    
    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 currentVelocity = rb2D.velocity;
        Vector2 newVelocity = currentVelocity;

        float directionX = (targetPosition - rb2D.position).normalized.x;
        
        directionX = directionX > 0.0f ? 1.0f : -1.0f;
        
        newVelocity.x = directionX * settings.moveSpeed * Time.fixedDeltaTime;
        
        rb2D.velocity = Vector2.Lerp(currentVelocity, newVelocity, settings.moveAcceleration);
        
        if (rb2D.velocity.y < settings.maxFallSpeed)
        {
            float clampedYSpeed = Mathf.Clamp(rb2D.velocity.y, -settings.maxFallSpeed, settings.maxFallSpeed);
            rb2D.velocity = new Vector2(rb2D.velocity.x, clampedYSpeed);
        }

        ChangeDirection(directionX);
        
        if (rb2D.velocity.x != 0.0f)
        {
            OnMove?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ChangeDirection(float direction)
    {
        transform.localScale = new Vector3(-direction, 1.0f, 1.0f);
    }

    public void StopMoving()
    {
        rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        
        OnMoveStop?.Invoke(this, EventArgs.Empty);
    }
    

    #endregion

    private void Attack()
    {
        if (Vector2.Distance(transform.position, playerController.transform.position) <= settings.attackRange &&
            !isAttacking)
        {
            canAttack = false;
            isAttacking = true;

            Collider2D[] results = new Collider2D[1];
            int numberOfResults = Physics2D.OverlapCircleNonAlloc(attackPoint.position, 0.1f, results, LayerMask.GetMask("Player"));
            
            if (numberOfResults > 0)
            {
                foreach (Collider2D result in results)
                {
                    if (result.TryGetComponent(out DamageTrigger damageTrigger))
                    {
                        damageTrigger.TakeDamage(settings.attackDamage);
                    }
                }
            }
        
            OnAttack?.Invoke(this, EventArgs.Empty); 
        }
    }
    
    private float GetSqrDistanceToPlayer()
    {
        return Vector2.SqrMagnitude(transform.position - playerController.transform.position);
    }

    public void TakeDamage(int damage)
    {
        OnHit?.Invoke(this, EventArgs.Empty);
        
        healthData.TakeDamage(damage);

        if (healthData.CurrentHealth <= 0)
        {
            healthData.SetCurrentHealth(0);
            isDead = true;
            Vector2 direction = (Vector2)(transform.position - playerController.transform.position).normalized + Vector2.up;
            rb2D.AddForce(direction * 2f, ForceMode2D.Impulse);
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetPlayerInRange(bool value)
    {
        isPlayerInRange = value;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
