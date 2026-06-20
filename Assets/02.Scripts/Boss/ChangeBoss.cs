using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChangeBoss : MonoBehaviour
{
    internal bool iscapricorn;

    [SerializeField] private GameObject _boss1; // 기본(평소) 오브젝트
    [SerializeField] private GameObject _boss2;

    [SerializeField] private GameObject _taurusObj;
    [SerializeField] private GameObject _capricornObj;
    [SerializeField] private GameObject _libraObj;

    [SerializeField] private Transform[] _spawnPoints; // 공격 패턴 등장 위치 후보

    [SerializeField] private float waitBeforeAttack = 2f;
    [SerializeField] private float waitAfterAttack = 2f;

    [SerializeField] private TextMeshProUGUI _starText;

    private GameObject _mainObj;          // 현재 평소 모습으로 쓰이는 오브젝트
    private GameObject _currentActiveObj; // 현재 활성화되어 있는 오브젝트(메인이든 공격이든)
    private PlayerMovement _pm;
    [SerializeField]private BossHealthsystem _bh;

    [SerializeField] private Image _typeImage;

    [Header("Attack1")]
    [SerializeField] private Sprite _taurusI;
    [SerializeField] private GameObject _taurus;
    [SerializeField] private Transform[] _boss_a1FireTransform;
    [SerializeField] private float _taurusSpawnInterval = 2f; // 좌우 한 쌍 생성 간격
    [SerializeField] private Image[] _warning;
    [SerializeField] private float _warningTime = 1f;

    [Header("Attack2")]
    [SerializeField] private Sprite _LibraI;
    [Header("Attack3")]
    [SerializeField] private Sprite _capricornI;
    [SerializeField] private GameObject[] _capricorn_tune;

    private void Start()
    {
        _mainObj = _boss1;
        _currentActiveObj = _mainObj;
        BossEffect.Instance.CloseEye();
        _starText.text = "MainBoss";

        _pm = FindFirstObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if (_bh._isDead) gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        
        StartCoroutine(AttackLoop());
       BossW.instance.LaunchInRandomDirection();
    }

    public void OnBoss1Defeated()
    {
        _mainObj = _boss2;

        if (_currentActiveObj == _boss1)
        {
            SwitchTo(_boss2);
        }
    }

    private IEnumerator AttackLoop()
    {
        while (!_bh._isDead)
        {
            yield return new WaitForSeconds(waitBeforeAttack);

            int attackNumber;
            if (iscapricorn)
            {
                attackNumber = Random.Range(1, 4); // 1~3
            }
            else
            {
                attackNumber = Random.value < 0.5f ? 1 : 3; // 1 또는 3
            }
            
            switch (attackNumber)
            { 
                case 1:
                    if (_bh._isDead)
                        yield break;

                    BossEffect.Instance.CloseEye();
                    _starText.text = "Taurus";
                    _typeImage.gameObject.SetActive(true);
                    _typeImage.sprite = _taurusI;
                    Taurus();
                    break;
                case 2:
       
                    if (iscapricorn)
                    {
                        if (_bh._isDead)
                            yield break;
                        BossEffect.Instance.CloseEye();
                        _starText.text = "Capricorn";
                        _typeImage.gameObject.SetActive(true);
                        _typeImage.sprite = _capricornI;
                        Capricorn();
                    }
                    break;
                case 3:
                    if (_bh._isDead)
                        yield break;
                    BossEffect.Instance.CloseEye();
                    _starText.text = "Libra";
                    _typeImage.gameObject.SetActive(true);
                    _typeImage.sprite = _LibraI;
                    Libra();
                    break;


            }

            yield return new WaitUntil(() => _currentActiveObj == null || !_currentActiveObj.activeSelf);

            BossEffect.Instance.CloseEye();
            
            SwitchTo(_mainObj);

            yield return new WaitForSeconds(waitAfterAttack);
        }
    }

    public void Taurus()
    {
        StartCoroutine(TaurusRoutine());
    }


    private IEnumerator TaurusRoutine()
    {
        if (_currentActiveObj != null && _currentActiveObj != _taurusObj)
        {
            _currentActiveObj.SetActive(false);
        }

        _taurusObj.SetActive(true);
        _currentActiveObj = _taurusObj;

        while (_taurusObj.activeSelf)
        {
            // 경고 표시
            foreach (Image warning in _warning)
            {
                warning.enabled = true;
            }

            yield return new WaitForSeconds(_warningTime);

            // 경고 숨김
            foreach (Image warning in _warning)
            {
                warning.enabled = false;
            }

            // 황소 생성
            foreach (Transform point in _boss_a1FireTransform)
            {
                if (point == null) continue;

                GameObject runner = Instantiate(_taurus, point.position, point.rotation);

                TaurusRunner runnerComp = runner.GetComponent<TaurusRunner>();
                if (runnerComp != null)
                {
                    runnerComp.Init(transform);
                }
            }

            yield return new WaitForSeconds(_taurusSpawnInterval);
        }
    }

    public void Capricorn()
    {
        SwitchToAtRandomPosition(_capricornObj);
        StartCoroutine(CapricornStart());
    }
    public IEnumerator CapricornStart()
    {
        int randomTune;
        for (int i = 0; i <= _capricorn_tune.Length; i++)
        {
            randomTune = Random.Range(0, _capricorn_tune.Length);

            yield return new WaitForSeconds(1f);
            Instantiate(_capricorn_tune[randomTune], _capricornObj.transform.position, Quaternion.identity);
        }

    }

    public void Libra()
    {

        SwitchToAtRandomPosition(_libraObj);
        LibraStart();
    }

    public void LibraStart()
    {
        if (_libraObj.activeSelf == true) _pm.IsReverseControl = true;
    }
    // 공격 패턴 오브젝트 전용: 랜덤 위치로 이동 후 전환
    private void SwitchToAtRandomPosition(GameObject nextObj)
    {
        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            nextObj.transform.position = point.position;
            nextObj.transform.rotation = point.rotation;
        }

        SwitchTo(nextObj);
    }

    private void SwitchTo(GameObject nextObj)
    {

        if (_currentActiveObj != null && _currentActiveObj != nextObj)
        {
            if (_currentActiveObj == _libraObj)
            {
                _pm.IsReverseControl = false; // libra가 꺼지는 순간 디버프 해제
            }
            _currentActiveObj.SetActive(false);
        }

        if (nextObj == _mainObj)
        {
            _starText.text = "MainBoss";
            _typeImage.gameObject.SetActive(false);
        }

        nextObj.SetActive(true);
        _currentActiveObj = nextObj;
    }
}