using System;
using System.Collections;
using Unity.AI;
using UnityEngine;
using UnityEngine.AI;
public class EnemyLogicisolstionTes : MonoBehaviour
{
    /// <summary>
    /// This script was created to serve as a simpler aproach to NPC navigation.
    /// </summary>
    
    private NavMeshAgent agent;

    [SerializeField]private bool isAttacking;
    [SerializeField] private bool isAttackOne_Gaurd;

    [SerializeField] private bool isAttackingTwo;

    [SerializeField] private bool isPatrolling;
    [SerializeField] private bool isPatrolling_Gaurd;

    public Transform[] WayPoint_AttackRoute1;
    [SerializeField] private int Attackindex;

    public Transform[] WayPoint_PatrollingRoute;
    [SerializeField] private int Patrollindex;

   

    //funkyyy
    [SerializeField]private bool IsAttackReady;
    Func<bool> IsAttackReady_Func ;

    //Link to other scripts
    //public PlayerHealth PlayerHealth_Script;


    private Coroutine StateSwitcherCourotine;

   #region EnemyAttackingState IEnumerator apraoch
    private void Start()
    {
        isAttackOne_Gaurd = true;
        isPatrolling_Gaurd = false;
        agent = GetComponent<NavMeshAgent>();
           // isAttacking = true;
    //conditions for Attack state
        if (StateSwitcherCourotine == null && isAttackOne_Gaurd == false )
        {
            StateSwitcherCourotine = StartCoroutine(AttackOneLogic());
        }

     //conditions for patroll state
        if (StateSwitcherCourotine == null && isPatrolling_Gaurd == false)
        {
            StateSwitcherCourotine = StartCoroutine(PatrollRouteLogic());
        }
    }
    private void Update()
    {
     StateSwitcherCourotine = null;
     Debug.Log(agent.destination);
     Debug.DrawLine(transform.position, agent.destination, Color.red);  
    }

 //Patrolling logic
    private IEnumerator PatrollRouteLogic()
    {
        while (isPatrolling == true)
        {
            Patrollindex = (Patrollindex + 1) % WayPoint_PatrollingRoute.Length;
            agent.SetDestination(WayPoint_PatrollingRoute[Patrollindex].position);
            Debug.Log("PATROLL");
            yield return new WaitForSeconds(1);
        }
        agent.ResetPath();
      yield return new WaitForSeconds(1);
    }

//Attacking Logic
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
                Attack();
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
    private void Attack()
    {
        agent.isStopped = true;
        if (agent.isStopped == true)
        {
           // PlayerHealth_Script.TakeDamage();
            Debug.Log("Attacked");
            agent.isStopped = false;
        }


    }

    #endregion
}