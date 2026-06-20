using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    internal SpriteRenderer _sr;
    internal bool _isFlip;
    internal bool _isTp = true;
    internal bool _isGround = false;
    internal bool _isTimeLine = false;

    internal Vector2 _dir;
    private Rigidbody2D _rb;

    [SerializeField] private float _speed;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float rayLenth;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _upGravity;
    [SerializeField] private float _downGravity;
    public bool IsReverseControl;

    [Header("텔레포트 쿨타임")]
    [SerializeField] private float _tpCooldown = 3f;
    [SerializeField] private Image _cooldownImage;
    public float _tpTimer = 0f;

    public readonly int HashisMove = Animator.StringToHash("isMove");
    public readonly int HashisJump = Animator.StringToHash("isJump");

    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isTimeLine) return;

        CooldownTick();
        MoveChecker();
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Tp();
    }

    private void CooldownTick()
    {
        if (_tpTimer <= 0f) return;

        _tpTimer -= Time.deltaTime;
        _cooldownImage.fillAmount = _tpTimer / _tpCooldown;

        if (_tpTimer <= 0f)
        {
            _tpTimer = 0f;
            _isTp = true;
            _cooldownImage.fillAmount = 0f;
        }
    }

    private void Tp()
    {
        if (!_isTp) return;

        transform.position = _target.position;
        _isTp = false;
        _tpTimer = _tpCooldown;
        _cooldownImage.fillAmount = 1f;
    }

    // 나머지 기존 코드 동일
    private void Jump()
    {
        _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
        _rb.gravityScale = 5;
        _rb.gravityScale = 1;
    }

    private void MoveChecker()
    {
        RaycastHit2D hit = Physics2D.Raycast(_groundChecker.position, Vector2.down, rayLenth, mask);
        Debug.DrawRay(_groundChecker.position, Vector2.down * rayLenth, Color.yellow);
        _isGround = hit.collider != null;
    }

    private void Move()
    {
        if (_dir.x != 0)
        {
            _rb.linearVelocityX = _dir.x * _speed;
            if (_isGround == false) return;
            _animator.SetBool(HashisMove, true);
        }
        else
        {
            _animator.SetBool(HashisMove, false);
        }
    }

    public void FlipByAim(bool faceLeft)
    {
        _sr.flipX = faceLeft;
        Vector3 pos = _target.localPosition;
        float posX = Mathf.Abs(pos.x);
        pos.x = faceLeft ? -posX : posX;
        _target.localPosition = pos;
    }

    private void OnMove(InputValue value)
    {
        _dir = value.Get<Vector2>();

        if (IsReverseControl)
            _dir.x *= -1;
    }
}
