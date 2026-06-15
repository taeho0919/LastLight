using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class MoveState : IState
{
    private MonsterSystem _monsterSystem;
    private Transform _transform;
    private Rigidbody2D _rb;

    public MoveState(MonsterSystem monster) => _monsterSystem = monster;

    public void Enter()
    {
        _monsterSystem._animator.SetBool(_monsterSystem.HasMove, true);

        _transform = _monsterSystem.transform;
        _rb = _monsterSystem.GetComponent<Rigidbody2D>();

        if (_monsterSystem._monsterData._speed <= 0)
        {
            _monsterSystem.SwitchAttackState();
        }
    }

    // MoveState.cs - X축만 이동
    public void Update()
    {

        if (_monsterSystem.target == null)
        {
            _monsterSystem.SwitchIdleState();
            return;
        }

        if (_monsterSystem._monsterData._attackType == AttackType.Guard)
        {
            Vector2 dir2 = Vector2.left;
          
            _rb.linearVelocity = new Vector2(
          dir2.x * _monsterSystem._monsterData._speed,
          _rb.linearVelocity.y
      );
            return;
        }
        Vector2 dir = ((Vector2)_monsterSystem.target.position - _rb.position).normalized;

        // 플립
        if (Mathf.Abs(dir.x) > 0.1f)
        {
            _transform.localScale = new Vector3(dir.x > 0 ? -1 : 1, 1, 1);
        }

        // X축만 이동, Y는 현재 velocity 유지 (중력 적용)
        _rb.linearVelocity = new Vector2(
            dir.x * _monsterSystem._monsterData._speed,
            _rb.linearVelocity.y
        );
    }

    public void Exit()
    {
        _rb.linearVelocity = Vector2.zero; // 상태 나갈 때 속도 초기화

        _monsterSystem._animator.SetBool(_monsterSystem.HasMove, false);

    }
}
