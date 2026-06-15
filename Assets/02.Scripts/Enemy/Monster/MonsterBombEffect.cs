using Unity.VisualScripting;
using UnityEngine;

public class MonsterBombEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            Destroy(gameObject);
        }
    }
}
