using DG.Tweening;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Stage3Manager : MonoBehaviour
{
    public static Stage3Manager Instance;
    [SerializeField] private CinemachineCamera _camera;

    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        Instance = this;
        _noise = _camera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        // 시작부터 켜두되 투명하게
    }
    public void ShakeStart(float intensity = 3f)
    {
        _noise.AmplitudeGain = intensity;
    }

    public void ShakeStop()
    {
        _noise.AmplitudeGain = 0f;
    }



}
