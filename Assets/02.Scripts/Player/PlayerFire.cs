using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerFire : MonoBehaviour
{
    [Header("총알 세팅")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private int _pelletCount = 10;
    [SerializeField] private float _spreadAngle = 30f;
    [SerializeField]private float _fireDelayTime = 1f;
    private bool _waitingForLand = false;
    private bool isReLoad = false;
    [SerializeField]private int fireCount;
    private int currentCount = 0;

    [Header("라이트")]
    [SerializeField] private Light2D _muzzleLight;
    [SerializeField] private float _flashDuration = 0.05f; // 추가
    [SerializeField] private float _flashIntensity = 3f;   // 추가

    [Header("샷건 반동")]
    [SerializeField] private float _recoilForce = 5f;
    [SerializeField] private float _vrecoilForce = 2f;
    [SerializeField] private float _recoilDuration = 0.1f;  // 반동 가속 지속 시간
    [SerializeField] private float _recoilRecovery = 0.2f;  // 복귀 시간
    [SerializeField] private PlayerMovement _pm;

    [Header("샷건 사운드")]
    [SerializeField]private AudioSource _audioSource;

    private Coroutine _recoilCoroutine;

    [Header("샷건 탄피")]
    [SerializeField]private GameObject _shellPrefab;
    [SerializeField] private Transform _shellPos;
    [SerializeField] private float _shelllifeTime;

    [Header("카메라 흔들림")]
    [SerializeField] private CinemachineImpulseSource _cis;

    public Stack<GameObject> shellPool = new Stack<GameObject>();
    public Stack<GameObject> bulletPool = new Stack<GameObject>();
    public static PlayerFire instance;
    public int RemainingShots => fireCount - currentCount; 
    public int MaxShots => fireCount;
    public bool IsReload=>isReLoad;

    private Rigidbody2D _rb;
    private void Awake()
    {
        instance = this;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_pm._isTimeLine) return;

        if (Input.GetKeyDown(KeyCode.R) && !isReLoad && currentCount > 0)
        {
            isReLoad = true;
            currentCount = 0;
            StartCoroutine(ReLoad()); // 공중이든 지상이든 즉시 재장전 시작
        }


        if (currentCount < fireCount)
        {
            if (isReLoad == false && Input.GetKeyDown(KeyCode.Mouse0))
            {
                _audioSource.Play();
                Shoot();
                Shake();
            }

        }
        else
        {
            isReLoad = true;
            currentCount = 0;
            if (_pm._isGround)
            {
                StartCoroutine(ReLoad());
            }
            else
            {
                if (!_waitingForLand)
                {
                    _waitingForLand = true;
                    currentCount = 0;
                    isReLoad = true;
                    StartCoroutine(WaitForLandThenReload());
                }
            }
        }
    }

    public void Shoot()
    {
        if (currentCount < fireCount)
            currentCount++;

        StartCoroutine(MuzzleFlash());

        for (int i = 0; i < _pelletCount; i++)
        {
            GameObject bullet;

            if (bulletPool.Count > 0)
            {
                bullet = bulletPool.Pop();
                bullet.SetActive(true);
            }
            else
            {
                bullet = Instantiate(_bulletPrefab, gameObject.transform);
            }

            float angle = Random.Range(-_spreadAngle / 2, _spreadAngle / 2);
            Quaternion rotation = _fireTransform.rotation * Quaternion.Euler(0, 0, angle);
            bullet.transform.position = _fireTransform.position;
            bullet.transform.rotation = rotation;
        }



        StartCoroutine(EjectShell());
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        Vector2 fireDir = _fireTransform.right;
        Vector2 recoilDirection = -fireDir;

        // 수직 반동 보정
        recoilDirection = new Vector2(
            recoilDirection.x * _recoilForce,
            recoilDirection.y * _vrecoilForce
        );

        // 기존 반동 중지
        if (_recoilCoroutine != null)
            StopCoroutine(_recoilCoroutine);

        // 즉시 밀어버림
        _rb.AddForce(recoilDirection, ForceMode2D.Impulse);

        _recoilCoroutine = StartCoroutine(RecoilCoroutine());
    }

    private IEnumerator RecoilCoroutine()
    {
        _rb.gravityScale = 0.5f;

        yield return new WaitForSeconds(_recoilRecovery);

        _rb.gravityScale = 1f;

        _recoilCoroutine = null;
    }
    
    private IEnumerator EjectShell()
    {
        GameObject shell;

        if(shellPool.Count > 0)
        {
            shell=shellPool.Pop();
            shell.transform.position =_shellPos.position;
            shell.SetActive(true);
        }
        else
        {
            shell = Instantiate(_shellPrefab, _shellPos.position, _shellPos.rotation, gameObject.transform);
        }

        Rigidbody2D rb= shell.GetComponent<Rigidbody2D>();
        rb.linearVelocity= Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(new Vector2(Random.Range(1f,4f),Random.Range(1f,3f)),ForceMode2D.Impulse);
             //AddTorque는 회전
        rb.AddTorque(Random.Range(200f,500f));

        yield return new WaitForSeconds(_shelllifeTime);
        shell.SetActive(false);
        shellPool.Push(shell);
    }
    private IEnumerator MuzzleFlash()
    {
        _muzzleLight.intensity = _flashIntensity;
        _muzzleLight.enabled = true;
        yield return new WaitForSeconds(_flashDuration);
        _muzzleLight.enabled = false;
    }
    private IEnumerator WaitForLandThenReload()
    {
        // 땅에 닿을 때까지 대기
        yield return new WaitUntil(() => _pm._isGround);

        // 착지 후 딜레이
        yield return new WaitForSeconds(0.5f);

        isReLoad = false;
        _waitingForLand = false;
    }
    private IEnumerator ReLoad()
    {
        yield return new WaitForSeconds(_fireDelayTime);
        isReLoad = false;
    }
    public void Shake()
    {
        Vector2 dir = -_fireTransform.right;
        _cis.GenerateImpulse(dir);
    }

}
