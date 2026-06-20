using Unity.Cinemachine;
using UnityEngine;

public class TaurusRunner : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime = 5f;
    
    private bool _running=false;
    private Transform _target;
    private Vector3 _direction;
    private bool _initialized;

    public void Init(Transform target, float? speedOverride = null)
    {
        _target = target;

        if (speedOverride.HasValue)
            _speed = speedOverride.Value;

        if (_target != null)
        {
            Vector3 dir = _target.position - transform.position;
            dir.y = 0f;
            _direction = dir.normalized;

            Vector3 scale = transform.localScale;
            scale.x = (_direction.x < 0) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;

            transform.localScale = scale;
        }
        else
        {
            _direction = Vector3.right;
        }

        _initialized = true;
    }

    private void OnEnable()
    {
        if (_lifeTime > 0f)
            Invoke(nameof(SelfDestruct), _lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (!_initialized)
            return;

        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_running)
            return;

        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(1, _direction);
                _running = true;
            }
        }
    }
}
