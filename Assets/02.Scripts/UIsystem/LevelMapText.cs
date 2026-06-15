using TMPro;
using UnityEngine;

public class LevelMapText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _guidelineText;
    [SerializeField] private float _fadeSpeed = 2f;

    private bool _isPlayerInside = false;

    void Update()
    {
        if (_isPlayerInside)
            _guidelineText.alpha = Mathf.MoveTowards(_guidelineText.alpha, 1f, _fadeSpeed * Time.deltaTime);
        else
            _guidelineText.alpha = Mathf.MoveTowards(_guidelineText.alpha, 0f, _fadeSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _isPlayerInside = false;
    }
}

