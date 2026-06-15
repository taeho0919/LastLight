using System.Collections;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttackState : IState
{
    private MonsterSystem _monsterSystem;
    private PlayerHealth _playerHealth;
    private float _attackTimer;
    
    public AttackState(MonsterSystem monsterSystem) => _monsterSystem = monsterSystem;

    public void Enter()
    {
        

    }
    public void Update()
    {
        if (_monsterSystem._monsterData._attackType == AttackType.Bomb)
        {
           BombAttack(); // ✅
        }
        else if (_monsterSystem._monsterData._attackType == AttackType.Range)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer > 0f) return;

            RangeAttack();

            _attackTimer = _monsterSystem._monsterData.rangeStats._delayTime;
        }
        
    }
    public void Exit() {
        _monsterSystem._animator.SetBool(_monsterSystem.HasMove,false);

    }

    private void BombAttack()
    {
        
            
            Collider2D hit = Physics2D.OverlapCircle(_monsterSystem.transform.position,
                _monsterSystem._monsterData.bombStats.explosionRadius,
                _monsterSystem._targetlayerMask);

        if (hit != null)
        {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>() ?? hit.GetComponentInParent<PlayerHealth>();
                Object.Instantiate(_monsterSystem._monsterData.bombStats._bombEffect,
                _monsterSystem.transform.position,
                Quaternion.identity);
                Object.Destroy(_monsterSystem.gameObject);

                 Vector2 hitDir = ph.transform.position - _monsterSystem.transform.position;
                ph.TakeDamage(1, hitDir);

                
        }


    }
    private void RangeAttack()
    {
        _monsterSystem._animator.SetBool(_monsterSystem.HasMove, true);

        GameObject obj = GameObject.Instantiate(
               _monsterSystem._monsterData.rangeStats._projectilePrefab,
               _monsterSystem._fireTransform.position,
               Quaternion.identity);

        if (obj.TryGetComponent<MonsterBullet>(out var bullet))
        {
            bullet.Init(_monsterSystem._monsterData.rangeStats._projectileSpeed);
        }
    }


}
