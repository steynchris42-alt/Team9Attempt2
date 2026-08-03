using Mono.Cecil.Cil;
using System.Data;
using UnityEngine;

public class EnemyBaseType : EnemyBluePrint
{
 #region RUNTIME
protected override void Update()
 {
base.Update();
 }
    #endregion
    #region StateMachine
    //What actually happens when each state is called. Mostly used to reset the destination.
    protected override void StateLogic(EnemyMoveState_Enum UpdateState)
    {
        Move_State = UpdateState;
        switch (UpdateState)
        {
            case EnemyMoveState_Enum.chasing:
                agent.ResetPath();
            break;
            case EnemyMoveState_Enum.patrolling:
                agent.ResetPath();
            break;
            case EnemyMoveState_Enum.updating:
                agent.ResetPath();
            break;
            case EnemyMoveState_Enum.AttackMove1:
                agent.ResetPath();
            break;
        }
    }
    protected override void HandleStateSwitches()
    {
        if (DistanceToPlayer <= 100)
        {
            if (Move_State != EnemyMoveState_Enum.chasing)
            {
             StateLogic(EnemyMoveState_Enum.chasing);
            }
            ChasingLogic();
        }
        else if (agent.pathPending == false && DistanceToPlayer > 100)
        {
            if (Move_State != EnemyMoveState_Enum.patrolling)
            {
                StateLogic(EnemyMoveState_Enum.patrolling);
            }
            PatrollingLogic();
        }
        if (DistanceToPlayer <= 10)
        {
            if (Move_State != EnemyMoveState_Enum.AttackMove1)
            {
                StateLogic(EnemyMoveState_Enum.AttackMove1);
            }
            AttackOne();
        }

    }

    #endregion
    #region MovementLogic
    protected override void ChasingLogic()
    {
    agent.destination = Player.position;
    switch (agent.remainingDistance)
      {
     case >= 30: agent.speed = MoveSpeedFast; break;
     case >= 20: agent.speed = MoveSpeedNormal; break;
     //case > 10: agent.speed = MoveSpeedSlow; break;
      }
    }
 
    protected override void PatrollingLogic()
    {
       agent.speed = MoveSpeedNormal;
        if (agent.pathPending == false && agent.remainingDistance <= 2)
        {
            iWaypointsB1Index = (iWaypointsB1Index + 1) % WayPointsRoute1.Length;
            agent.destination = WayPointsRoute1[iWaypointsB1Index].position;
        }
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
    #region COMBAT
    protected override void AttackOne()
    {
        index_AttackWroute1 = Random.Range(0, WayPointsRoute1.Length); //int now returns a random index value of the Transform array :>
        agent.destination = WayPointsRoute1[index_AttackWroute1].position; //The destination will be random on each switch, because the randomised int is being used as the index peramater.
    }

    #endregion
}

//===========================================================
//ARCHIVES OFf OLD CODE///
//===================================================
#region STATE SWITCH FIRST ATTEMPT

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
#region STATE SWITCH SECOND ATTEMPT
/*** ==CODE START HERE==
 *    protected virtual void Update()
    {
        DistanceToPlayer = Vector3.Distance(Player.position , transform.position);
        if (DistanceToPlayer <= 100)
        {
            if (Move_State != EnemyMoveState_Enum.chasing)
            {
                MovementStateSwitch(EnemyMoveState_Enum.chasing);
            }
            ChasingLogic();
        }
        else if (DistanceToPlayer > 100)
        {
            if (Move_State != EnemyMoveState_Enum.patrolling)
            {
                MovementStateSwitch(EnemyMoveState_Enum.patrolling);
            }
        PatrollingLogic();
        }
        }
 * ==CODE END HERE==***/
/*
        ==NOTES==
  Problem: This aproach allowed me to mimic the bool self cheack from before, excpet using enums. Although this does work fine
  for a small amount of state switches, it is unboptimal for teh purpose of handling a vast amount of switches.

  Solution:
To ensure that a clean architecture is established, I will create a new method that will handle the distance cheacks and state switches.
I will then use a switch statement in update to link "MovementStateSwitch(EnemyMoveState_Enum)" and the state switch method.
Doing this will divide my code neatly imnto blocks, offering a far more streamlined setup that I can mange as the complexity groes.
*/
#endregion
#region GLOBAL ENUM VS PARAMETER
/* protected override void HandleStateSwitches()
  {
      if (DistanceToPlayer <= 100)
      {
          if (Move_State != EnemyMoveState_Enum.chasing)
          {
              //StateLogic(EnemyMoveState_Enum.chasing);
              Move_State = EnemyMoveState_Enum.chasing;
              GlobalEnumAproach();
          }
          ChasingLogic();
      }
      else if (agent.pathPending == false && DistanceToPlayer > 100)
      {
          if (Move_State != EnemyMoveState_Enum.patrolling)
          {
              // StateLogic(EnemyMoveState_Enum.patrolling);
              Move_State = EnemyMoveState_Enum.patrolling;
              GlobalEnumAproach();
          }
          PatrollingLogic();
      }
  }
  protected override void GlobalEnumAproach()
  {
      switch (Move_State)
      {
          case EnemyMoveState_Enum.chasing:
              agent.ResetPath();
              break;
          case EnemyMoveState_Enum.patrolling:
              agent.ResetPath();
              break;
          case EnemyMoveState_Enum.updating:
              agent.ResetPath();
              break;
      }
  }
using the global enum in this way works, but it requires the method to be called with the line that changes the enum value.
Declaring it as a parameter for a method makes it so that the enum can be changed when the method is called in teh parameter brackets.

*/
#endregion

