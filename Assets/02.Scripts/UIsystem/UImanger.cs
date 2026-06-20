using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanger : MonoBehaviour
{
    [Header("총알")]
    [SerializeField] private TextMeshProUGUI _bulletText;
    [Header("HP바")]
    [SerializeField] private Image _hpImage;
    [SerializeField] private Sprite[] _hpSprites;   // 인덱스 0 = HP5(최대), 5 = HP0(사망)

    [Header("세팅")]
    [SerializeField] private GameObject _escPenal;
    [SerializeField] private GameObject _settingPenal;
    private bool isOn=false;

    private float _inputDelay = 4f;
    private float _inputTimer = 0f;
    private bool _inputReady = false;

    [Header("사운드")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private PlayerFire _playerFire;
    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;

    private void Awake()
    {

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerFire= player.GetComponent<PlayerFire>();
            _playerHealth = player.GetComponent<PlayerHealth>();
            _playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    private void Start()
    {
        if (_playerHealth != null)
            _playerHealth.OnHpChanged += HpBar;
        HpBar(_playerHealth != null ? _playerHealth.PlayerHP : 0);

        // 저장된 볼륨 불러오기
        _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        _bgmSource.volume = _bgmSlider.value;
        _sfxSource.volume = _sfxSlider.value;

        // 슬라이더 이벤트 연결
        _bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        if (_playerHealth != null)
            _playerHealth.OnHpChanged -= HpBar;

        // 슬라이더 이벤트 해제
        _bgmSlider.onValueChanged.RemoveListener(OnBGMChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
    }
    public void OnBGMChanged(float value)
    {
        _bgmSource.volume = value;
        PlayerPrefs.SetFloat("BGMVolume", value); // 게임 껐다 켜도 유지
    }

    public void OnSFXChanged(float value)
    {
        _sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }


    private void Update()
    { 


        Bullet();
        // 입력 준비 타이머
        if (!_inputReady)
        {
            _inputTimer += Time.deltaTime;
            if (_inputTimer >= _inputDelay)
                _inputReady = true;
            return; // 준비 전엔 아무것도 안 함
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {


            if (_settingPenal.activeSelf)
            {
                OnClickSettingClose();
            }
            else if (_escPenal.activeSelf)
            {
                _playerMovement._isTimeLine = false;
                _escPenal.SetActive(false);
            }
            else
            {
                _playerMovement._isTimeLine = true;
                _escPenal.SetActive(true);
            }
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickSetting()
    {
        _escPenal.SetActive(false);
        _settingPenal.SetActive(true );
       
    }
    public void OnClickSettingClose()
    {
        _escPenal.SetActive(true);
        _settingPenal.SetActive(false);
    }
    public void Bullet()
    {
        if (_playerFire == null) return;

        _bulletText.text = _playerFire.IsReload
            ? $"0 / {_playerFire.MaxShots}"
            : $"{_playerFire.RemainingShots} / {_playerFire.MaxShots}";
    }

    // HP 값을 받아 스프라이트 교체
    public void HpBar(int currentHp)
    {
        if (_hpImage == null || _hpSprites == null || _hpSprites.Length == 0) return;

        // HP 5→[0], HP 4→[0], HP 3→[1], HP 2→[2], HP 1→[3], HP 0→[4]
        int maxHp = 5;
        int index = Mathf.Clamp(maxHp - currentHp, 0, _hpSprites.Length - 1);
        _hpImage.sprite = _hpSprites[index];
    }
}
