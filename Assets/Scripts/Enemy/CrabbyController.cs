using System;
using UnityEngine;

public class CrabbyController : MonoBehaviour, IDamageable, IMoveable, IAggroable
{
    [Header("Configuration")]
    [SerializeField] private EnemyDataSO settings;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Rigidbody2D rb2D;
    private int currentHealth;
    private bool isPlayerInRange;
    private PlayerController playerController;
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (isPlayerInRange)
        {
            MoveTowards(playerController.transform.position);
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
    #endif

    #region Movement 
    
    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 currentVelocity = rb2D.velocity;

        float directionX = (targetPosition - rb2D.position).normalized.x;
        
        directionX = directionX > 0.0f ? 1.0f : -1.0f;
        
        float moveVelocityX = settings.moveSpeed * directionX;
        rb2D.velocity = Vector2.Lerp(currentVelocity, new Vector2(moveVelocityX, currentVelocity.y), settings.moveAcceleration * Time.fixedDeltaTime);

        if (rb2D.velocity.x < 0.0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb2D.velocity.x > 0.0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void StopMoving()
    {
        rb2D.velocity = Vector2.zero;
    }
    

    #endregion

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
}
