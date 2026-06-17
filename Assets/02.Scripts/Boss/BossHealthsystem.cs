using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SubsystemsImplementation;


public enum BossType
{
    Main,
    Sub
}

public class BossHealthsystem : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private BossType _bossType;

    [Header("Sub Boss Only")]
    [SerializeField] private BossHealthsystem _mainBoss;
    [SerializeField] private int _bonusDamageToMain;

    private int _currHp;
    private bool _isDead;

    private void OnEnable()
    {
        _currHp = _hp;
        _isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead || damage <= 0) return;

        _currHp -= damage;
        if (_currHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        if (_bossType == BossType.Sub && _mainBoss != null)
        {
            DieCubeManager.instance.SpawnDieCube(gameObject.transform.position,100);
            _mainBoss.TakeDamage(_bonusDamageToMain);
        }

        // TODO: 사망 연출(애니메이션, 사운드, 이펙트)은 나중에 여기 추가
        gameObject.SetActive(false);

        if (_bossType == BossType.Main)
        {
           
            SceneManager.LoadScene("EndScene");
        }
    }
}
