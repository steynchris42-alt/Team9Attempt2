using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Enemy_RegularType_Child : Enemy_parent_Class
{
    //--Overrides
    private Coroutine State_Tracker_Ranger_Coro;
   public override Coroutine State_Tracker_Coro  //overrides parent coroutine with child specific coroutine
    {
    get => State_Tracker_Ranger_Coro;
    set => State_Tracker_Ranger_Coro = value;
     }
    //-- ANimation Related
    [Header("Animation Related")]
    public Animator Ranger_Anim;

    public Transform Shank_base;
    public bool isAttacking;
    public Func<bool> IsAttackReady_Func;
    public bool IsAttackReady;

    public Enemy_Shoot_mech shoot_scr;
    public Shooting PlayerShootScr;
    //Func bool setup for patrolling coroutine
    public Func<bool> is_Chasing_del;

  
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
        if (Ranger_Anim != null)
        {
            Ranger_Anim.SetBool("isFlying", isPatrolling || isChasing); //Enables FLYING animation when chansing and patrolling; 
            Ranger_Anim.SetBool("isAttacking", isAttacking);
        }

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

        if (isPatrolling == true && State_Tracker_Ranger_Coro == null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            Debug.Log("Should start patrolling");
            State_Tracker_Ranger_Coro = StartCoroutine(PatrollRouteLogic());
        }
        else
           if (isChasing == true && State_Tracker_Ranger_Coro == null)
        {
            Debug.Log("Should start chasing");
            State_Tracker_Ranger_Coro = StartCoroutine(ChasingLogic());
        }
    }
    private IEnumerator PatrollRouteLogic()
    {
        while (isPatrolling == true  && IsDead == false && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            Debug.Log("RANGER PATROLL");
          
            {
                if (isChasing)
                {
                    agent.ResetPath();
                    yield break;
                }
            }
            Patrollindex = UnityEngine.Random.Range(0, WayPoint_PatrollingRoute.Length);
            agent.SetDestination(WayPoint_PatrollingRoute[Patrollindex].position);
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance
            && !agent.pathPending || isChasing);
        }
        agent.ResetPath();
        State_Tracker_Ranger_Coro = null;
        yield return null;
    }
    private IEnumerator ChasingLogic()
    {
        if (isPatrolling == true && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
            yield break;
        }
        Debug.Log("CHASE");
        while (isChasing == true && isPatrolling == false && IsDead == false && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
          //  Ranger_Anim.SetBool("isFlying", true); // starts FYLING animation
          //  Ranger_Anim.SetBool("isIdle", false); // stops idle animation 
            agent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.5f);
            if (Is_ShootingDistance_())
            {
                isAttacking = true;
                shoot_scr.ShootingLogic();
            }
            else if(Is_ShootingDistance_() == false)
            {
                isAttacking = false;
            }
        }
        agent.ResetPath();
        State_Tracker_Ranger_Coro = null;
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
         
        if (Dis_to_Player <= 30.0f && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
           // Debug.Log("isInShankRange TRUE");
            return true;
        }
        else
        {
            if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }
            //Debug.Log("isInShankRange TRUE");
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

        State_Tracker_Ranger_Coro = null;
        yield return null;
    }
public override void TakeDamage()
    {
        base.TakeDamage();
    }
}

//------ARCHIVE------///
