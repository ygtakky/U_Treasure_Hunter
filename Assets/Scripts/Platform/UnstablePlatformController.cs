using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnstablePlatformController : MonoBehaviour
{
    [SerializeField] private UnstablePlatformDataSO settings;
    
    [Header("References")]
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform sprite;
    
    private bool isDisabled;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisablePlatform());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isDisabled)
            {
                StopAllCoroutines();
            }
        }
    }

    private IEnumerator DisablePlatform()
    {
        Vector3 originalPosition = sprite.localPosition;
        float elapsed = 0f;
        
        while (elapsed < settings.shakeDuration)
        {
            sprite.localPosition = originalPosition + Random.insideUnitSphere * settings.shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        sprite.localPosition = originalPosition;
        
        isDisabled = true;
        platform.SetActive(false);
        
        yield return new WaitForSeconds(settings.respawnTime);
        
        platform.SetActive(true);
        isDisabled = false;
    }
}
