using Unity.Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerHP = 5;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public int PlayerHP => _playerHP;

    // HP 변경 시 UIManager에 알리는 이벤트
    public event System.Action<int> OnHpChanged;

    private bool _isDead;

    public int TakeDamage(int damage, Vector2 hitDirection)
    {
        if (_isDead) return 0;

        _playerHP -= damage;
        _rb.AddForce(hitDirection.normalized * _knockbackForce, ForceMode2D.Impulse);
        _impulseSource.GenerateImpulse();
        StartCoroutine(FlashRed());

        // 이벤트 발생 → UIManager.HpBar() 호출
        OnHpChanged?.Invoke(_playerHP);

        if (_playerHP <= 0)
        {
            _isDead = true;
            Destroy(gameObject);
            SceneManager.LoadScene("Death Scene");
        }

        return _playerHP;
    }

    public void Heal(int healAmount)
    {
        if (_playerHP >= 5) return;

        _playerHP += healAmount;
        OnHpChanged?.Invoke(_playerHP);  // 힐 시에도 이벤트 발생
    }

    private IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = Color.white;
    }
}
