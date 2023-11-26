using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData settings;
    
    [Header("Grounding Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isJumping;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGrounded();
        
        Debug.Log(isGrounded);
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
    }
        

    private void FixedUpdate()
    {
        if (isJumping && isGrounded)
        {
            Jump();
        }
        
        isJumping = false;
        
        Vector2 currentVelocity = rb.velocity;
        Vector2 newVelocity = currentVelocity;
        newVelocity.x = horizontalInput * settings.movementSpeed * Time.fixedDeltaTime;

        rb.velocity = Vector2.Lerp(currentVelocity, newVelocity, 10.0f);
    }
    
    private void Jump()
    {
        rb.AddForce(Vector2.up * settings.jumpForce, ForceMode2D.Impulse);
    }
    
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
