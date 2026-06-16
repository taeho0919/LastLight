using UnityEngine;
using UnityEngine.Playables;

public class TimeLine : MonoBehaviour
{
    [SerializeField] private PlayableDirector _pd;
    [SerializeField] private PlayerMovement _pm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pm._isTimeLine = true;
        _pd.Play();
    }
}
