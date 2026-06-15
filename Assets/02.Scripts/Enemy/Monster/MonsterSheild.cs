using System.Collections;
using UnityEngine;

public class MonsterSheild : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
           collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Damage(collision));
        }
    }

    private IEnumerator Damage(Collision2D col)
    {
        if (col.collider.TryGetComponent<PlayerHealth>(out var health))
        {
            
            health.TakeDamage(1, Vector2.left);
            
            yield return new WaitForSeconds(1);
            
            
        }
    }
}
