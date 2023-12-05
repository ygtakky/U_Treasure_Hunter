using System;
using UnityEngine;

public class CrabbyController : MonoBehaviour, IDamageable, IMoveable, IAggroable
{
    public event EventHandler OnMove;
    public event EventHandler OnMoveStop;
    public event EventHandler OnAttack;
    
    [Header("Configuration")]
    [SerializeField] private EnemyDataSO settings;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform attackPoint;
    
    private Rigidbody2D rb2D;
    private int currentHealth;
    private bool isPlayerInRange;
    private bool isAttacking;
    private bool canAttack = true;
    private PlayerController playerController;
    private float attackTimer;
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
    }
    
    private void Update()
    {
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
        currentHealth = settings.maxHealth;
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

        float directionX = (targetPosition - rb2D.position).normalized.x;
        
        directionX = directionX > 0.0f ? 1.0f : -1.0f;
        
        float moveVelocityX = settings.moveSpeed * directionX;
        rb2D.velocity = Vector2.Lerp(currentVelocity, new Vector2(moveVelocityX, currentVelocity.y), settings.moveAcceleration * Time.fixedDeltaTime);

        ChangeDirection();
        
        if (rb2D.velocity.x != 0.0f)
        {
            OnMove?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ChangeDirection()
    {
        if (rb2D.velocity.x < 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (rb2D.velocity.x > 0.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    public void StopMoving()
    {
        rb2D.velocity = Vector2.zero;
        
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
            Physics2D.OverlapCircleNonAlloc(attackPoint.position, 0.1f, results, LayerMask.GetMask("Player"));
            
            if (results[0] != null)
            {
                results[0].GetComponent<DamageTrigger>()?.TakeDamage(settings.attackDamage);
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
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            // TODO: Add enemy death logic
        }
        
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");
    }

    public void SetPlayerInRange(bool value)
    {
        isPlayerInRange = value;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
}
