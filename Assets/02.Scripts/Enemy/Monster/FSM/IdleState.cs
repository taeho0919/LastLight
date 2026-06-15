using UnityEngine;

public class IdleState : IState
{
    private MonsterSystem _monsterSystem;
    public IdleState(MonsterSystem monsterSystem)=>_monsterSystem = monsterSystem;

    public void Enter() { 
   
    }
    public void Update() { }
    public void Exit() { }
}
