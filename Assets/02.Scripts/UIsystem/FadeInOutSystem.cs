using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FadeInOutSystem : MonoBehaviour
{
    public static FadeInOutSystem Instance { get; private set; }
     public CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private void Awake()
    {
        Instance = this;

        if (_canvasGroup == null)
        {
            Debug.LogError($"{name}: _canvasGroup이 할당되지 않았습니다!", this);
        }

        _canvasGroup.gameObject.SetActive(false);
    }

    public void FadeIn(System.Action onComplete = null)
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1f, _fadeDuration).OnComplete(() => onComplete?.Invoke());
    }

    public void FadeOut(System.Action onComplete = null)
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(() =>
        {
            onComplete?.Invoke();
            _canvasGroup.gameObject.SetActive(false);
        });
    }
}

