using Mono.Cecil.Cil;
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


 
    protected override void MovementStateSwitch(EnemyMoveState_Enum UpdateState)
    {
        //
        if (Move_State == UpdateState)
        {
            return;
        }
        Move_State = UpdateState;
        switch (UpdateState)
        {
            case EnemyMoveState_Enum.chasing: 
            break;
            case EnemyMoveState_Enum.patrolling:
                agent.destination = WayPointsRoute1[1].position;
            break;
            case EnemyMoveState_Enum.updating: break;
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
        //Debug.Log("CHASING");
    }
 
    protected override void PatrollingLogic()
    {
       agent.speed = MoveSpeedNormal;
        if (agent.pathPending == false && agent.remainingDistance <= 2)
        {
            iWaypointsB1Index = (iWaypointsB1Index + 1) % WayPointsRoute1.Length;
            agent.destination = WayPointsRoute1[iWaypointsB1Index].position;
        }
        //Debug.Log("PATROLLING");
    }

    protected override bool IsChasing()
    {
        throw new System.NotImplementedException();
    }
    protected override bool IsPatrolling()
    {
       throw new System.NotImplementedException();
    }
    #endregion
 #region NpcManagemnet
    protected override void SpawnLogic()
    {
        return;
    }  
    
    #endregion
}

//===========================================================
//ARCHIVES OFf OLD CODE///
//===================================================
#region STATE SWITCH FIRST ATTEMPt

/*** ==CODE START HERE==
 * 
 * protected override void MovementStateSwitch()
  {
      DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
      switch (DistanceToPlayer)
      {
          case < 100:
              ChasingLogic();
              IsStateSwitching = true;
              break;
          case > 100:
              if (IsStateSwitching == true)
              {
                  
                  IsStateSwitching = false;
              }
              PatrollingLogic();
              break;
      }
  }
 ==CODE END HERE==***/
/*
        ==NOTES==
  Problem:
Initially I just used the distance cheack as the condition to switch states. This resulted in the NPC only returning to the patrol route 
- after reaching the last position of the player prior to the state switch.
  Solution:
I used a switch statement to determine when the enemy and player came within 100 units of eachother.
This moment acted as the peramitor for the state switch which to occurr. I enabled the state switch by-
disabling a bool the exact frame after the first point in the array was set as the new destination. 
This procedure only plays out once due to its condition being when teh bool is enabled. 
By having this only playout a single time, the patrolling logic is able to ensue seamlessley. 
  Reason for being scrapped:
ALthough this worked fine, it can't be optimally used for MULTIPlE state switches. 
And so i decided to use a pre established enum


logic to play out 

*/


#endregion
