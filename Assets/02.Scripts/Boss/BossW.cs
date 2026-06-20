using System.Runtime.CompilerServices;
using UnityEngine;

public class BossW : MonoBehaviour
{
      public float _rollSpeed = 10;
    [SerializeField] private Vector3 rollAxis = Vector3.right; // X축, Z축이면 Vector3.forward
 
    [Header("Launch / Bounce 설정")]
    [SerializeField] private float minLaunchSpeed = 5f;   // 튕길 때 최소 속도
    [SerializeField] private float maxLaunchSpeed = 15f;   // 튕길 때 최대 속도
    [SerializeField] private float bounceAngleVariance = 20f; // 반사 후 흔들어줄 랜덤 각도 범위(도)
 
    public static BossW instance;
 
    private Rigidbody2D rb;
 
    private void OnEnable()
    {
        instance = this;
    }
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
 
    private void Update()
    {
        transform.Rotate(rollAxis * _rollSpeed * Time.deltaTime);
    }
 
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent<PlayerHealth>(out PlayerHealth component);
            component.TakeDamage(1, rollAxis);
            
        }
        if (collision.CompareTag("Wall"))
        {
            // 트리거는 물리 접점 정보가 없으므로, Ground 콜라이더에서
            // 내 위치와 가장 가까운 표면 점을 구해 그 방향을 법선으로 근사
            Vector2 myPos = rb.position;
            Vector2 closestPoint = collision.ClosestPoint(myPos);
            Vector2 normal = myPos - closestPoint;
 
            // 완전히 내부에 들어와 있어서 거리가 0이면, 이동 방향의 반대를 임시 법선으로 사용
            normal = normal.sqrMagnitude < 0.0001f
                ? -rb.linearVelocity.normalized
                : normal.normalized;
 
            BounceOffSurface(normal);
        }
    }
 
    public void LaunchInRandomDirection()
    {
        // XY 평면에서 랜덤한 방향 (0~360도)
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
 
        // 튕길 때마다 랜덤한 속도
        float speed = Random.Range(minLaunchSpeed, maxLaunchSpeed);
 
        rb.linearVelocity = direction * speed;
    }
 
    private void BounceOffSurface(Vector2 normal)
    {
        // 들어온 방향의 반대(반사 벡터) 계산: 입사각 = 반사각
        Vector2 incoming = rb.linearVelocity;
 
        // 혹시 속도가 0인 상태로 부딫혔을 경우를 대비한 안전장치
        if (incoming.sqrMagnitude < 0.0001f)
        {
            incoming = -normal;
        }
 
        Vector2 reflected = Vector2.Reflect(incoming.normalized, normal);
 
        // 완전한 거울 반사는 너무 예측 가능하므로, 약간의 랜덤 각도를 더해 흔들어줌
        float randomAngle = Random.Range(-bounceAngleVariance, bounceAngleVariance);
        Vector2 finalDirection = Rotate(reflected, randomAngle);
 
        // 튕길 때마다 랜덤한 속도
        float speed = Random.Range(minLaunchSpeed, maxLaunchSpeed);
 
        rb.linearVelocity = finalDirection.normalized * speed;
    }
 
    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
