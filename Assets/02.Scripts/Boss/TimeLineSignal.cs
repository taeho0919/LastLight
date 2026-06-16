using UnityEngine;

public class TimeLineSignal : MonoBehaviour
{
    [SerializeField] private BossText _tx;
    [SerializeField] private PlayerMovement _pm;
    [SerializeField] private BossW bw;

    public void StartRoll()
    {
        bw._rollSpeed = 1000f;
    }

    public void StartText()
    {
        _tx.StartText();
    }

    public void TimeLineEnd()
    {
        _pm._isTimeLine = false;
    }
}
