using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class Enemy_HunterType_Child : Enemy_parent_Class
{
    private Coroutine State_Tracker_Hunter_Coro;
    public override Coroutine State_Tracker_Coro  //overrides parent coroutine with child specific coroutine
    {
        get => State_Tracker_Hunter_Coro;
        set => State_Tracker_Hunter_Coro = value;
    }
    // public Transform Shank_base;
   // public Coroutine Hunter__Coro;
    public bool isAttacking;
    public Func<bool> IsAttackReady_Func;
    public bool IsAttackReady;
    public Transform FirePoint;

 //public Enemy_Shoot_mech shoot_scr;
 //public Shooting PlayerShootScr;
 //Func bool setup for patrolling coroutine
 
    public Func<bool> is_Chasing_del;
    public void Awake()
    {
        base.Awake();
       
    }
    public void Start()
    {
        base.Start();
    }
    public void Update()
    {
      base.Update();
      FirePoint.transform.LookAt(player.transform.position);
        switch (Dis_to_Player)
        {
            case >= 100.0f:
                if (IsDead == false)
                {
             isPatrolling = true;
             isChasing = false; 
                }
              break;
            case <= 100.0f:
                if (IsDead == false)
                {
            isPatrolling = false;
             isChasing = true;
                    
                 }
             break;
            }

        if (isPatrolling == true && State_Tracker_Hunter_Coro == null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            Debug.Log("Should start patrolling");
            State_Tracker_Hunter_Coro = StartCoroutine(PatrollRouteLogic());
        }
        else
           if (isChasing == true && State_Tracker_Hunter_Coro == null)
        {
            Debug.Log("Should start chasing");
            State_Tracker_Hunter_Coro = StartCoroutine(Hunter_Attack_Basic()); //--Initiat ATTACK or CHASE --//
        }
    }
    private IEnumerator PatrollRouteLogic()
    {
        Debug.Log("PATROLL");
      
        while (isPatrolling == true  && IsDead == false && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            if (isChasing == true)
            {
            agent.ResetPath();
            yield break;
            }
            Patrollindex = UnityEngine.Random.Range(0, WayPoint_PatrollingRoute.Length);
            agent.SetDestination(WayPoint_PatrollingRoute[Patrollindex].position);
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance
            && !agent.pathPending || isChasing);
        }
        agent.ResetPath();
        State_Tracker_Hunter_Coro = null;
        yield return new WaitForSeconds(1);
    }

    private bool Is_Stabbing_Distance_()
    {
         
        if (Dis_to_Player <= 30.0f && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
     isAttacking=true;
            return true;
        }
        else
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                isAttacking = false;
            }
            return false;
        }

    }
    private IEnumerator Hunter_Attack_Basic() //Attack pattern for hunter enemy type
    {
        while (isChasing == true)
        {
           // Debug.Log("Hunter attack chace");
          agent.SetDestination(player.transform.position);
            if (!agent.pathPending && agent.remainingDistance <= 10)
            {
            agent.isStopped = true;
          //  Debug.Log("Stab!");
            yield return new WaitForSeconds(1);
            agent.ResetPath();
            agent.isStopped = false;
          //  Debug.Log("stop stab!");
            }
        yield return null;

        }
    }
    private IEnumerator Hunter_Attack() 
    {
        Debug.Log("Hunter_Attack START");
        while (isChasing == true)
        { 
            if (!agent.pathPending && agent.remainingDistance <= 2)
            {
            Debug.Log("Start_Stabbing");
            agent.SetDestination(player.transform.position);
              if (agent.remainingDistance <= 5)
                {
                 Debug.Log("Stab StaB STAB!");
                 yield return new WaitForSeconds(3);
                 agent.ResetPath();
             }
            Attackindex = UnityEngine.Random.Range(0, WayPoint_AttackRoute1.Length);
            agent.SetDestination(WayPoint_AttackRoute1[Attackindex].position);
            }
            yield return null;
        }
        State_Tracker_Hunter_Coro = null;      
    }
   

    public void Hunter_AttackMove()
    {
      agent.SetDestination(player.transform.position);
        if (agent.remainingDistance <= 5)
        {
            Debug.Log("Stab StaB STAB!");
        }
      agent.ResetPath();
    }
public override void TakeDamage()
    {
        base.TakeDamage();
    }
}

//------ARCHIVE------///
/*   private IEnumerator ChasingLogic()
   {
       if (isPatrolling == true && agent.isActiveAndEnabled && agent.isOnNavMesh)
       {
           agent.ResetPath();
           yield break;
       }
       Debug.Log("CHASE");
       while (isChasing == true && isPatrolling == false && IsDead == false && agent.isActiveAndEnabled && agent.isOnNavMesh)
       {
           agent.SetDestination(player.transform.position);
           yield return new WaitForSeconds(0.5f);
           if (Is_ShootingDistance_())
           {
               isAttacking = true;
               ShootingLogic();
           }
           else if(Is_ShootingDistance_() == false)
           {
               isAttacking = false;
           }
       }
       agent.ResetPath();
       State_Tracker_Coro = null;
       yield return null;    
   } */

/*  private IEnumerator AttackPlayer()
  {

      while (Is_ShootingDistance_() && isPatrolling == false && IsDead == false)
      {

          isAttacking = true;
         ShootingLogic();
        yield return new WaitForSeconds(5);

      }
      //agent.isStopped = false;
  } */
