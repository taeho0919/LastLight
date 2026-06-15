using UnityEngine;
using UnityEngine.UI;

public class TpChecker : MonoBehaviour
{
    [SerializeField]private Image _skillImage;
    [SerializeField] private Sprite _changeImage;
    [SerializeField] private Sprite _diffultImage;
    private bool _isWall;
    PlayerMovement _pm;
    private void Awake()
    {
        _pm = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (_pm._tpTimer > 0f) return; // 쿨타임 중이면 건드리지 않음
        _pm._isTp = !_isWall;

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")){ 
        _skillImage.sprite = _changeImage;
        _isWall = true;
         }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            _skillImage.sprite = _diffultImage;
            _isWall = false;
        }
    }
}
