
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    public Func<bool> is_Chasing_del;

    public void Start()
    {
        base.Start();
    }

    public void Update()
    {
        base.Update();
        shoot_scr.FirePoint.transform.LookAt(player.transform.position);
        Is_ShootingDistance_();

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
        else if (isChasing == true && State_Tracker_Coro == null)
        {
            Debug.Log("Should start chasing");
            State_Tracker_Coro = StartCoroutine(ChasingLogic());
        }
    }

    private IEnumerator PatrollRouteLogic()
    {
        Debug.Log("PATROLL");

        while (isPatrolling == true && IsDead == false)
        {
            if (isChasing == true)
            {
                if (agent != null && agent.enabled && agent.isOnNavMesh)
                {
                    agent.ResetPath();
                }
                yield break;
            }

            Patrollindex = UnityEngine.Random.Range(0, WayPoint_PatrollingRoute.Length);

            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(WayPoint_PatrollingRoute[Patrollindex].position);
            }

            // Fixed: Added safety checks to prevent 'GetRemainingDistance' error
            yield return new WaitUntil(() =>
                isChasing ||
                (agent != null && agent.enabled && agent.isOnNavMesh &&
                 agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            );
        }

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
        State_Tracker_Coro = null;
        yield return new WaitForSeconds(1);
    }

    private IEnumerator ChasingLogic()
    {
        if (isPatrolling == true)
        {
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.ResetPath();
            }
            yield break;
        }
        Debug.Log("CHASE");
        while (isChasing == true && isPatrolling == false && IsDead == false)
        {
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(player.transform.position);
            }

            yield return new WaitForSeconds(0.5f);

            if (Is_ShootingDistance_())
            {
                isAttacking = true;
                shoot_scr.ShootingLogic();
            }
            else if (Is_ShootingDistance_() == false)
            {
                isAttacking = false;
            }
        }

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
        State_Tracker_Coro = null;
        yield return null;
    }

    private IEnumerator AttackPlayer()
    {
        while (Is_ShootingDistance_() && isPatrolling == false && IsDead == false)
        {
            isAttacking = true;
            shoot_scr.ShootingLogic();
            yield return new WaitForSeconds(5);
        }
    }

    private bool Is_ShootingDistance_()
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            if (Dis_to_Player <= 30.0f)
            {
                agent.isStopped = true;
                Debug.Log("isInShankRange TRUE");
                return true;
            }
            else
            {
                agent.isStopped = false;
                Debug.Log("isInShankRange FALSE");
                return false;
            }
        }
        return false;
    }

    private IEnumerator AttackOneLogic()
    {
        yield return new WaitForSeconds(0.2f);
        IsAttackReady_Func = () => IsAttackReady;
        while (isAttacking == true)
        {
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(WayPoint_AttackRoute1[Attackindex].position);

                // Fixed: Added safety check here as well to prevent any lingering remainingDistance errors
                if (!agent.pathPending && agent.remainingDistance <= 2 && IsAttackReady == false)
                {
                    Attackindex = UnityEngine.Random.Range(0, WayPoint_AttackRoute1.Length);
                    IsAttackReady = true;
                    yield return new WaitUntil(IsAttackReady_Func);
                    IsAttackReady = false;
                }
                if (IsAttackReady == true)
                {
                    agent.ResetPath();
                }
            }
            yield return null;
        }
    }

    private IEnumerator StateSwitch()
    {
        if (IsStateSwitch_Over == false)
        {
            Debug.Log("STATE SWITCH");
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.ResetPath();
            }
            yield return new WaitForSeconds(1);
            IsStateSwitch_Over = true;
        }

        State_Tracker_Coro = null;
        yield return null;
    }
}