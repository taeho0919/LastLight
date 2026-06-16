using UnityEngine;

public class BossW : MonoBehaviour
{
    public float _rollSpeed = 10;
    [SerializeField] private Vector3 rollAxis = Vector3.right; // X축, Z축이면 Vector3.forward

    private void Update()
    {
        transform.Rotate(rollAxis * _rollSpeed * Time.deltaTime);
    }
}
