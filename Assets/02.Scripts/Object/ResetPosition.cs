using UnityEngine;
using UnityEngine.Rendering;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] private Transform _ResetPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {
           collision.transform.position = _ResetPosition.position;
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1, Vector2.zero);
        }
    }
}
