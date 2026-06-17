using UnityEngine;
using System.Collections;

public class ChangeBoss : MonoBehaviour
{
    [SerializeField] private GameObject _boss1; // 기본(평소) 오브젝트
    [SerializeField] private GameObject _boss2;

    [SerializeField] private GameObject _taurusObj;
    [SerializeField] private GameObject _capricornObj;
    [SerializeField] private GameObject _libraObj;

    [SerializeField] private Transform[] _spawnPoints; // 공격 패턴 등장 위치 후보

    [SerializeField] private float waitBeforeAttack = 2f;
    [SerializeField] private float waitAfterAttack = 2f;

    private GameObject _mainObj;          // 현재 평소 모습으로 쓰이는 오브젝트
    private GameObject _currentActiveObj; // 현재 활성화되어 있는 오브젝트(메인이든 공격이든)

    private void Start()
    {
        _mainObj = _boss1;
        _currentActiveObj = _mainObj;
        
    }

    private void OnEnable()
    {
        StartCoroutine(AttackLoop());
    }

    public void OnBoss1Defeated()
    {
        _mainObj = _boss2;

        if (_currentActiveObj == _boss1)
        {
            SwitchTo(_boss2);
        }
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitBeforeAttack);

            int attackNumber = Random.Range(1, 4); // 1~3

            switch (attackNumber)
            {
                case 1:
                    Taurus();
                    break;
                case 2:
                    Capricorn();
                    break;
                case 3:
                    Libra();
                    break;
            }

            yield return new WaitUntil(() => _currentActiveObj == null || !_currentActiveObj.activeSelf);

            SwitchTo(_mainObj);

            yield return new WaitForSeconds(waitAfterAttack);
        }
    }

    public void Taurus()
    {
        SwitchToAtRandomPosition(_taurusObj);
    }

    public void Capricorn()
    {
        SwitchToAtRandomPosition(_capricornObj);
    }

    public void Libra()
    {
        SwitchToAtRandomPosition(_libraObj);
    }

    // 공격 패턴 오브젝트 전용: 랜덤 위치로 이동 후 전환
    private void SwitchToAtRandomPosition(GameObject nextObj)
    {
        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            Transform point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            nextObj.transform.position = point.position;
            nextObj.transform.rotation = point.rotation;
        }

        SwitchTo(nextObj);
    }

    private void SwitchTo(GameObject nextObj)
    {
        Debug.Log($"[SwitchTo] {nextObj?.name} 활성화 시도, 이전: {_currentActiveObj?.name}");

        if (_currentActiveObj != null && _currentActiveObj != nextObj)
        {
            _currentActiveObj.SetActive(false);
        }

        nextObj.SetActive(true);
        _currentActiveObj = nextObj;

        Debug.Log($"[SwitchTo] 결과 - activeSelf: {nextObj.activeSelf}, activeInHierarchy: {nextObj.activeInHierarchy}");
    }
}