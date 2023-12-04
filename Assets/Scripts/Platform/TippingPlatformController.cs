using System;
using System.Collections;
using UnityEngine;

public class TippingPlatformController : MonoBehaviour
{
    [SerializeField] private TippingPlatformDataSO settings;
    
    private Rigidbody2D rb2D;
    private float initialRotation;
    private const string PLAYER_TAG = "Player";
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        initialRotation = rb2D.rotation;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollision(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        HandleCollision(other.gameObject);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            StartCoroutine(ResetPlatformRotation());
        }
    }
    
    private void HandleCollision(GameObject obj)
    {
        if (obj.CompareTag(PLAYER_TAG)) 
        {
            StopAllCoroutines();
            
            float playerDot = GetPlayerDot(obj);
            float angularVelocity = -playerDot * settings.rotatingForce;
            SetAngularVelocity(angularVelocity);
        }
    }
    
    private float GetPlayerDot(GameObject player)
    {
        Transform platformTransform = transform;
        return Vector2.Dot(player.transform.position - platformTransform.position, platformTransform.right);
    }
    
    private void SetAngularVelocity(float angularVelocity)
    {
        rb2D.angularVelocity = angularVelocity;
    }
    
    private IEnumerator ResetPlatformRotation()
    {
        SetAngularVelocity(0f);

        yield return new WaitForSeconds(settings.resetRotationDelaySeconds);
        
        while (Math.Abs(rb2D.rotation - initialRotation) > 0.1f)
        {
            rb2D.rotation = Mathf.MoveTowards(rb2D.rotation, initialRotation, settings.resetRotationSpeed * Time.deltaTime);
            yield return null;
        }
        
        rb2D.rotation = initialRotation;
        
        yield return null;
    }
}
