using DG.Tweening;
using System.Security.Cryptography;
using UnityEngine;

public class DieCube : MonoBehaviour
{
    [SerializeField]private float _time = 0.5f;
    private Vector3 _originalScale;
    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = _originalScale;

        transform
            .DOScale(Vector3.zero, _time)//(목표 크기,걸리는 시간)
            .SetEase(Ease.InBack);
        //Ease.Linear=일정 속도로 변함
        //Ease.InBack=끝으로 갈수록 빨라짐,살짝 튕기는 느낌
        //Ease.OutBack=처음에 빠르다가 끝에 튕기는 느낌
        //Ease.InOutBack=시작과 끝 모두 살짝 튕김
        //Ease.InBounce=끝에서 통통 튀는 느낌
    }
}
