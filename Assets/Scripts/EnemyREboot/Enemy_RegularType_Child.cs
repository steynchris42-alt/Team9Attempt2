using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Enemy_RegularType_Child : Enemy_parent_Class
{
    public GameObject Shank;
    public Transform Shank_base;
  public Coroutine State_Tracker_Coro;
   

    //Func bool setup for patrolling coroutine


    public void Start()
    {
      
        base.Start();
        
       // isPatrolling = true;
    }
    public void Update()
    {
        base.Update();
       //Shank.transform.LookAt(player.transform.position);
        Is_StabbingDistance_();
        //Assigning specific distances to bools.
        //That way I can use them as conditions for action in my Ineumerators.


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
        
        if (Is_StabbingDistance_() == true && isChasing == true )
        {
            StopCoroutine(ChasingLogic());
            State_Tracker_Coro = null;
            if (State_Tracker_Coro == null)
            {
                State_Tracker_Coro = StartCoroutine(AttackPlayer());
            }
        }
       
        
        
       
        //Debug.Log("I am alive");
    }
    private IEnumerator PatrollRouteLogic()
    {
       // isSwitch_ToPatrolling= false;
        Debug.Log("PATROLL");
        // isPatrollDone_func = () => isPatrollingDone; //Assigns the bool value to the delegate
        while (isPatrolling == true  && IsDead == false)
        {
            if (isChasing == true)
            {
            agent.ResetPath();
            yield break;
            }
            Patrollindex = (Patrollindex + 1) % WayPoint_PatrollingRoute.Length;
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
        if (Is_StabbingDistance_())
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
       
        Debug.Log("GET STABBED ");
        while (Is_StabbingDistance_() && isPatrolling == false && IsDead == false)
        {
            agent.isStopped = true;
          Shank.transform.position = Vector3.Lerp(Shank.transform.position, player.transform.position, 1f * Time.deltaTime);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;
    }

    private bool Is_StabbingDistance_()
    {
        if (Dis_to_Player <= 6.0f)
        {
            Debug.Log("isInShankRange TRUE");
            return true;
        }
        else
        {
            Debug.Log("isInShankRange TRUE");
            return false;
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
