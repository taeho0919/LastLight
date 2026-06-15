using UnityEngine;

public class HealPack : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerHealth>(out var playerHealth);
            playerHealth.Heal(1);
            Destroy(gameObject);
        }
        
    }
}
