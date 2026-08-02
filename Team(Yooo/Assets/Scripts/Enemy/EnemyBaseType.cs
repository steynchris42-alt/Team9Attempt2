using UnityEngine;

public class EnemyBaseType : EnemyBluePrint
{
    #region RUNTIME
    protected override void Update()
    {
        base.Update();
    }
    #endregion
    #region MovementLogic
    protected override void ChasingLogic()
    {
        agent.destination = Player.position;
        if (agent.remainingDistance > 10)
        {
            agent.speed = MoveSpeedNormal;
        }
        else if (agent.remainingDistance <= 10)
        {
            agent.speed = MoveSpeedSlow;
        }
    }
    protected override void PatrollingLogic()
    {
        
    }
    #endregion
}
