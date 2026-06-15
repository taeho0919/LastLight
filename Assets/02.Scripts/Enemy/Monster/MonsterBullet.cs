using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    private float _speed;

    public void Init(float speed)
    {
        _speed = speed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.left * _speed;

        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHealth>(out var health))
        {
            Vector2 hitDir = Vector2.left;
            health.TakeDamage(1, hitDir);
            Destroy(gameObject);
        }
    }
}
