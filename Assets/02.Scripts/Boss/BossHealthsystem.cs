using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum BossType
{
    Main,
    Sub,
    W
}

public class BossHealthsystem : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private BossType _bossType;

    [Header("MainBossOnly")]
    [SerializeField] private Image _hpBarImage;       // fillAmount로 채움/감소를 표현할 HP바 이미지 (Image Type: Filled)
    [SerializeField] private float _hpBarTweenDuration = 0.4f; // HP바가 줄어드는 데 걸리는 시간

    [Header("Sub Boss Only")]
    [SerializeField] private BossHealthsystem _mainBoss;
    [SerializeField] private int _bonusDamageToMain;

    [SerializeField] private int _currHp;
    public bool _isDead;
    private Tween _hpBarTween;
    private ChangeBoss _cb;
    private void OnEnable()
    {
        _cb = FindAnyObjectByType<ChangeBoss>();

        if (_bossType == BossType.Sub)
        {
           
            _currHp = _hp;
            _isDead = false;
        }
        // Main과 W는 둘 다 "아직 설정 안 된 경우(0 이하)에만" 보정.
        // 원래 코드는 연산자 우선순위 때문에 Main이면 무조건 덮어썼던 버그가 있어서 괄호로 명확히 묶음.
        else if ((_bossType == BossType.Main || _bossType == BossType.W) && _currHp <= 0)
        {
            _currHp = _hp;
        }
        
        UpdateHpBarImmediate();
    }
    private void Update()
    {
        if (_bossType == BossType.Main &&
            _currHp <= _hp / 2)
        {

            _cb.iscapricorn = true;
        }
    }
    public void TakeDamage(int damage)
    {
        if (_isDead || damage <= 0) return;

        _currHp -= damage;

        if (_bossType == BossType.Main)
        {
            UpdateHpBarAnimated();
        }

        if (_currHp <= 0)
        {
            Die();
        }
    }

    // HP바를 현재 값에 즉시 맞춤 (트윈 없이). 활성화 시점 등 초기화에 사용.
    private void UpdateHpBarImmediate()
    {
        if (_bossType != BossType.Main || _hpBarImage == null) return;

        _hpBarTween?.Kill();
        _hpBarImage.fillAmount = _hp > 0 ? (float)_currHp / _hp : 0f;
    }

    // 데미지를 입었을 때 DOTween으로 부드럽게 fillAmount를 줄임.
    private void UpdateHpBarAnimated()
    {
        if (_hpBarImage == null || _hp <= 0) return;

        float targetFill = Mathf.Clamp01((float)_currHp / _hp);

        _hpBarTween?.Kill();
        _hpBarTween = _hpBarImage
            .DOFillAmount(targetFill, _hpBarTweenDuration)
            .SetEase(Ease.OutQuad);
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        if (_bossType == BossType.Sub && _mainBoss != null)
        {
            DieCubeManager.instance.SpawnDieCube(gameObject.transform.position, 100);
            _mainBoss.TakeDamage(_bonusDamageToMain);
        }

        if (_bossType == BossType.W)
        {
            Destroy(gameObject);
        }


        if (_bossType == BossType.Main)
        {
            

            FadeInOutSystem.Instance.FadeIn(() =>
            {
                
                SceneManager.LoadScene("EndScene");
            });
        }

        // TODO: 사망 연출(애니메이션, 사운드, 이펙트)은 나중에 여기 추가
        gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        _hpBarTween?.Kill();
    }
}
