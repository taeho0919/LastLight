using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MonsterSystem : MonoBehaviour
{
     public MonsterData _monsterData;

    [SerializeField] private DieCubeManager _cubeManager;
    [SerializeField] private Transform _spawnTransform;

    public LayerMask _targetlayerMask;
    [SerializeField] private float _range;//추격 범위
    [SerializeField] private float _rangeA;//공격 범위
    public Transform _fireTransform;
    public Transform target;

    private IState _curState;
    private MoveState _moveState;
    private IdleState _idleState;
    private AttackState _attackState;

    private bool _isGuardChasing = false;
    private int _curhp;
    internal bool _isDead = false;

    public readonly int HasMove = Animator.StringToHash("IsMove");
    public Animator _animator;

    private void Awake()
    {

        var player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        _curhp =_monsterData._health;
        
        CheckAnimator();

        _moveState=new MoveState(this);
        _idleState=new IdleState(this);
        _attackState=new AttackState(this);
        
        ChangeState(_idleState);
    }

    private void FixedUpdate()
    {
        ChaseRange();
    }

    public void ChaseRange()
    {
        Collider2D chaseHit = Physics2D.OverlapCircle(transform.position, _range, _targetlayerMask);

        // Guard 타입이고 이미 추격 시작했으면 범위 밖이어도 계속 추격
        if (_monsterData._attackType == AttackType.Guard)
        {
            if (chaseHit != null && !_isGuardChasing)
            {
                _isGuardChasing = true; // 최초 감지 시 플래그 ON
            }

            if (!_isGuardChasing)
            {
                SwitchIdleState();
                return;
            }

            Collider2D attackHit = Physics2D.OverlapCircle(transform.position, _rangeA, _targetlayerMask);
            if (attackHit != null && _curState != _attackState&&!(_monsterData._attackType==AttackType.Guard)) SwitchAttackState();
            else if (attackHit == null && _curState != _moveState) SwitchMoveState();
            return;
        }

        // 기존 비Guard 타입 로직
        if (chaseHit == null)
        {
            SwitchIdleState();
            return;
        }

        Collider2D attackHitNormal = Physics2D.OverlapCircle(transform.position, _rangeA, _targetlayerMask);
        if (attackHitNormal != null && _curState != _attackState) SwitchAttackState();
        else if (attackHitNormal == null && _curState != _moveState) SwitchMoveState();
    }
    public void TakeDamage(int damage)
    {
        _curhp -= damage;
        if (_curhp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        _cubeManager.SpawnDieCube(
            _spawnTransform.position,
            _monsterData._chunkCount
        );

        Destroy(gameObject);
    }

    private void CheckAnimator()
    {
        if (_monsterData._attackType == AttackType.Bomb)
        {
            _animator.runtimeAnimatorController = _monsterData.bombStats._bombController;
        }
        else if (_monsterData._attackType == AttackType.Range)
        {
            _animator.runtimeAnimatorController = _monsterData.rangeStats._rangeController;
        }
        else
        {
            _animator.runtimeAnimatorController=_monsterData.guardStats._guardController;
        }
    }

    private void Update() => _curState?.Update();

    public void SwitchMoveState()=>ChangeState(_moveState);
    public void SwitchIdleState() => ChangeState(_idleState);
    public void SwitchAttackState() => ChangeState(_attackState);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,_range);

        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, _rangeA);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _monsterData.bombStats.explosionRadius);
    }

    public void ChangeState(IState newState)
    {
        if(_curState == newState||newState==null) return;

        _curState?.Exit();
        _curState = newState;
        _curState?.Enter();
    }
}
