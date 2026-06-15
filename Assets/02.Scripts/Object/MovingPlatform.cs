using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveAxis { Horizontal, Vertical }

    [Header("이동 설정")]
    public MoveAxis axis = MoveAxis.Horizontal;
    public float distance = 3f;
    public float duration = 1.2f;
    public Ease easeType = Ease.InOutSine;

    private Vector3 _origin;

    void Start()
    {
        _origin = transform.position;

        Vector3 target = axis == MoveAxis.Horizontal
            ? _origin + new Vector3(distance, 0f, 0f)
            : _origin + new Vector3(0f, distance, 0f);

        transform.DOMove(target, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void OnCollisionEnter2D(Collision2D col) => SetParent(col.transform, true);
    void OnCollisionExit2D(Collision2D col) => SetParent(col.transform, false);

    void SetParent(Transform t, bool attach)
    {
        t.SetParent(attach ? transform : null);
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}
