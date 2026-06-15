using Unity.Cinemachine;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Vector2 mouseOffset;
    [SerializeField] private PlayerMovement _pm;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Mouse();
    }

    private void Mouse()
    {
        if (_pm._isTimeLine) return;

        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 origin = (Vector2)gunPivot.position + mouseOffset;
        Vector2 dir = (Vector2)mousePos - origin;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle);

        bool shouldFlip = mousePos.x < transform.position.x;
        gunPivot.localScale = shouldFlip ? new Vector3(1, -1, 1) : Vector3.one;

        _pm.FlipByAim(shouldFlip);
    }
}