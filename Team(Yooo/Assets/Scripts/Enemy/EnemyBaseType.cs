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
    protected override void MovementStateSwitch()
    {
       float DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
        switch (DistanceToPlayer)
        {
            case >= 100: PatrollingLogic(); break;
            case < 100: ChasingLogic(); break;
        }
    }
  protected override void ChasingLogic()
  {
    agent.destination = Player.position;
    switch (agent.remainingDistance)
      {
     case >= 30: agent.speed = MoveSpeedFast; break;
     case >= 20: agent.speed = MoveSpeedNormal; break;
     case <= 10: agent.speed = MoveSpeedSlow; break;
      }
 }
    protected override void PatrollingLogic()
    {
        agent.speed = MoveSpeedNormal;
        if (WayPointsRoute1 == null) 
        {
            return; 
        }
        if (agent.remainingDistance <= 0)
        {
            iWaypointsB1Index = (iWaypointsB1Index + 1) % WayPointsRoute1.Length;
            agent.destination = WayPointsRoute1[iWaypointsB1Index].position;
        } 
    }
    #endregion
    #region NpcManagemnet
    protected override void SpawnLogic()
    {
        return;
    }  
    
    #endregion
}
