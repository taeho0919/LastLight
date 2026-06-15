using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PollutedAir : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Vector2 _boxSize = new Vector2(1f, 5f);
    [SerializeField] private Vector2 _boxOffset = Vector2.zero;
    [SerializeField] private LayerMask _filterLayer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _deathTime = 1f;

    private Tween _moveTween;
    private bool _isStopped = false;
    private Coroutine _deathCoroutine;
    private int _i = 0;
    private void Start()
    {
        if (Stage3Manager.Instance != null)
        {
            Stage3Manager.Instance.ShakeStart(3f);
        }

        _moveTween = transform.DOMoveX(999f, 999f / _moveSpeed);
    }

    private void Update()
    {


        if (_isStopped) return;

        Vector2 origin = (Vector2)transform.position + _boxOffset;

        // 필터 감지
        Collider2D hit = Physics2D.OverlapBox(origin, _boxSize, 0f, _filterLayer);
        if (hit != null)
        {
            _isStopped = true;
            Stage3Manager.Instance.ShakeStop();
            _moveTween.Kill();
            return;
        }

        // 플레이어 감지
        Collider2D hit2 = Physics2D.OverlapBox(origin, _boxSize, 0f, _playerLayer);
        if (hit2 != null)
        {
            // 플레이어가 안에 있고 코루틴 없으면 시작
            if (_deathCoroutine == null)
                _deathCoroutine = StartCoroutine(DeathCo(hit2.GetComponent<PlayerHealth>()));
        }
        else
        {
            // 플레이어가 나가면 타이머 초기화
            if (_deathCoroutine != null)
            {
                StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }
        }

    }

    private IEnumerator DeathCo(PlayerHealth ph)
    {

        yield return new WaitForSeconds(_deathTime);
        _i++;
        if (_i >= 5) _moveTween.Kill();
        ph.TakeDamage(1,Vector2.right);
        _deathCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 origin = (Vector2)transform.position + _boxOffset;
        Gizmos.DrawWireCube(origin, _boxSize);
    }
}