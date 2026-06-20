using DG.Tweening;
using UnityEngine;

public class Tune : MonoBehaviour
{
    [Header("추격 속도")]
    [SerializeField] private float _moveSpeed = 3f; // 플레이어를 향한 수평 추격 속도

    [Header("물결 움직임 (DOTween)")]
    [SerializeField] private float _waveDistance = 1.5f; // 위/아래로 움직이는 거리(중심 기준 ±)
    [SerializeField] private float _waveDuration = 0.6f; // 한쪽 끝까지 이동하는 데 걸리는 시간

    private Transform _playerTransform;
    private Rigidbody2D _rb;
    private Tween _waveTween;

    private float _waveOffsetY = 0f;
    private float _baseY;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        _baseY = _rb.position.y;
        StartWaveTween();
    }

    private void StartWaveTween()
    {
        _waveTween = DOTween.To(
                () => _waveOffsetY,
                x => _waveOffsetY = x,
                _waveDistance,
                _waveDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void FixedUpdate()
    {
        ChaseAndWave();
    }

    private void ChaseAndWave()
    {
        float newX = _rb.position.x;

        if (_playerTransform != null)
        {
            float dirX = Mathf.Sign(_playerTransform.position.x - _rb.position.x);
            newX += dirX * _moveSpeed * Time.fixedDeltaTime;

            // 흔들림의 중심선(_baseY)을 플레이어 Y좌표 쪽으로 서서히 보정.
            // 이게 없으면 플레이어가 위아래로 움직여도 출렁임 중심이 고정된 채라
            // 같은 높이로 절대 못 내려오는 문제가 생김.
            _baseY = Mathf.MoveTowards(_baseY, _playerTransform.position.y, _moveSpeed * Time.fixedDeltaTime);
        }

        float newY = _baseY + _waveOffsetY;

        _rb.MovePosition(new Vector2(newX, newY));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
                playerHealth.TakeDamage(1, hitDirection);

                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        _waveTween?.Kill();
    }


}
