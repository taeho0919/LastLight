using UnityEngine; // UnityEditor.Animations 제거

public enum AttackType { Bomb, Range, Guard }

[System.Serializable]
public class BombStats
{
    public float explosionRadius;
    public GameObject _bombEffect;
    public RuntimeAnimatorController _bombController; // ← 변경
}

[System.Serializable]
public class RangeStats
{
    public float _projectileSpeed;
    public float _delayTime;
    public GameObject _projectilePrefab;
    public AnimatorOverrideController _rangeController; // ← 이건 UnityEngine 소속이라 그대로 OK
}

[System.Serializable]
public class GuardStats
{
    public AnimatorOverrideController _guardController; // ← 이것도 그대로 OK
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "mdata")]
public class MonsterData : ScriptableObject
{
    public float _speed;
    public int _health;
    public int _chunkCount;
    public AttackType _attackType;
    public BombStats bombStats;
    public RangeStats rangeStats;
    public GuardStats guardStats;
}