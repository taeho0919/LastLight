using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 1f;
    private float _timer = 0f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            _timer = 0f;
            PlayerFire.instance.bulletPool.Push(gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            PlayerFire.instance.bulletPool.Push(gameObject);
            gameObject.SetActive(false);

                collision.GetComponent<MonsterSystem>().TakeDamage(20);
            
        }
    }
}
