using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FadeInOutSystem : MonoBehaviour
{
    public static FadeInOutSystem Instance { get; private set; }
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void FadeIn(System.Action onComplete = null)
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1f, _fadeDuration).OnComplete(() => onComplete?.Invoke());
    }

    public void FadeOut(System.Action onComplete = null)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(0f, _fadeDuration).OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });
    }
}

