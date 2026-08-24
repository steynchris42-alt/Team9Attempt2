using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public class Enemy_RegularType_Child : Enemy_parent_Class
{
    
    [SerializeField] private bool isPatrolling;
    [SerializeField] private Coroutine State_Tracker_Coro;

    //Func bool setup for patrolling coroutine
    Func<bool> isPatrollDone_func;
    [SerializeField] private bool isPatrollingDone;

  public void Start()
    {
        base.Start();
        isPatrolling = true;
    }
    public void Update()
    {
        if (State_Tracker_Coro == null && isPatrolling == true)
        {
            State_Tracker_Coro = StartCoroutine(PatrollRouteLogic());
        }
        base.Update();
        Debug.Log("I am alive");
    }
    private IEnumerator PatrollRouteLogic()
    {
       // isPatrollDone_func = () => isPatrollingDone; //Assigns the bool value to the delegate
        while (isPatrolling == true)
        {
            Patrollindex = (Patrollindex + 1) % WayPoint_PatrollingRoute.Length;
            agent.SetDestination(WayPoint_PatrollingRoute[Patrollindex].position);
            Debug.Log("PATROLL");
            yield return new WaitUntil(()=> agent.remainingDistance<=agent.stoppingDistance 
            && !agent.pathPending);
        }
     agent.ResetPath();
        yield return new WaitForSeconds(1);
    }
}

//------ARCHIVE------///
