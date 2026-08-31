using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Enemy_RegularType_Child : Enemy_parent_Class
{
    public Transform Shank_base;
  public Coroutine State_Tracker_Coro;

    public bool isAttacking;
    public Func<bool> IsAttackReady_Func;
    public bool IsAttackReady;

    public Enemy_Shoot_mech shoot_scr;





    //Func bool setup for patrolling coroutine


    public void Start()
    {
      
        base.Start();
        
       // isPatrolling = true;
    }
    public void Update()
    {
        base.Update();
        shoot_scr.FirePoint.transform.LookAt(player.transform.position);
        Is_ShootingDistance_();

        //Assigning specific distances to bools.
        //That way I can use them as conditions for action in my Ineumerators.

        shoot_scr.ShootingLogic();
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


        if (isPatrolling == true && State_Tracker_Coro == null)
        {
            Debug.Log("Should start patrolling");
            State_Tracker_Coro = StartCoroutine(PatrollRouteLogic());
        }
        else
           if (isChasing == true && State_Tracker_Coro == null)
        {
            Debug.Log("Should start chasing");
            State_Tracker_Coro = StartCoroutine(ChasingLogic());
        }
        
        if (Is_ShootingDistance_() == true && isChasing == true )
        {
            StopCoroutine(ChasingLogic());
            State_Tracker_Coro = null;
            if (State_Tracker_Coro == null)
            {
                isAttacking = true;
                State_Tracker_Coro = StartCoroutine(AttackPlayer());
            }
        }
      
    }
    private IEnumerator PatrollRouteLogic()
    {
      
        Debug.Log("PATROLL");
      
        while (isPatrolling == true  && IsDead == false)
        {
            if (isChasing == true)
            {
            agent.ResetPath();
            yield break;
            }
            Patrollindex = UnityEngine.Random.Range(0, WayPoint_PatrollingRoute.Length);
            agent.SetDestination(WayPoint_PatrollingRoute[Patrollindex].position);

            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance
            && !agent.pathPending);
       
        }
        agent.ResetPath();
        State_Tracker_Coro = null;
        yield return new WaitForSeconds(1);
    }
    private IEnumerator ChasingLogic()
    {
        if (Is_ShootingDistance_())
        {
            State_Tracker_Coro = null;
            yield break;
        }
        if (isPatrolling == true)
        {
            agent.ResetPath();
            yield break;
        }
        Debug.Log("CHASE");
        while (isChasing == true && isPatrolling == false && IsDead == false)
        {
         
            agent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.5f);
           
           
        }
        agent.ResetPath();
        State_Tracker_Coro = null;
        yield return null;
    }

    private IEnumerator AttackPlayer()
    {
       
        while (Is_ShootingDistance_() && isPatrolling == false && IsDead == false)
        {

            isAttacking = true;
            shoot_scr.ShootingLogic();
            
           // shank_Scr.shank_attack();
            yield return new WaitForSeconds(5);
            
        }
     
        //agent.isStopped = false;
    }

    private bool Is_ShootingDistance_()
    {
        if (Dis_to_Player <= 20.0f)
        {
            agent.isStopped = true;
            Debug.Log("isInShankRange TRUE");
            return true;
        }
        else
        {
            agent.isStopped = false;
            Debug.Log("isInShankRange TRUE");
            return false;
        }
    }

    private IEnumerator AttackOneLogic()
    {
        yield return new WaitForSeconds(0.2f);
        IsAttackReady_Func = () => IsAttackReady;
        while (isAttacking == true)
        {
            agent.SetDestination(WayPoint_AttackRoute1[Attackindex].position);
            if (!agent.pathPending && agent.remainingDistance <= 2 && IsAttackReady == false)
            {
                Attackindex = UnityEngine.Random.Range(0, WayPoint_AttackRoute1.Length);
                IsAttackReady = true;
              //  shank_Scr.shank_attack();
                yield return new WaitUntil(IsAttackReady_Func);
                IsAttackReady = false;
            }
            if (IsAttackReady == true)
            {
                agent.ResetPath();
            }
            yield return null;
        }
    }
    private IEnumerator StateSwitch()
    {
        if (IsStateSwitch_Over == false)
        {
            Debug.Log("STATE SWITCH");
            agent.ResetPath();
            yield return new WaitForSeconds(1);
            IsStateSwitch_Over = true;
        }

        State_Tracker_Coro = null;
        yield return null;
    }
}

//------ARCHIVE------///
