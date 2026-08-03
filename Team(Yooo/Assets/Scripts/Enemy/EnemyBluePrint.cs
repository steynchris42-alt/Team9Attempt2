using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public abstract class EnemyBluePrint : MonoBehaviour
{
    #region INSTANCE FIELDS
    [Header("NPC Management Settings")]
    #region NpcManageSettings
    //FOR TRACKING BASE ENEMY TYPE
    [SerializeField] protected int iEnemyBaseTypeTracker;
    virtual protected int iBaseTracker
    {
        get { return iEnemyBaseTypeTracker; }
        set { value = iEnemyBaseTypeTracker; }
    }
    //FOR TRACKING MOBILITY ENEMY TYPE
    [SerializeField] protected int iEnemyMobilityTypeTracker;
    virtual protected int iMobilityTracker
    {
        get { return iEnemyMobilityTypeTracker; }
        set { value = iEnemyMobilityTypeTracker; }
    }
    //FOR TRACKING Heavy ENEMY TYPE
    [SerializeField] protected int iEnemyHeavyTypeTracker;
    virtual protected int iHeavyTracker
    {
        get { return iEnemyHeavyTypeTracker; }
        set { value = iEnemyHeavyTypeTracker; }
    }

    #endregion

    [Header("Navmesh Settings")]
    #region NavmeshSettings
    //TARGETS
    [SerializeField] protected Transform Player;
    protected float DistanceToPlayer;
    //FOR PATROLLING STATE
    [SerializeField] protected Transform DestinationOutlier;
    [SerializeField] protected int iWaypointsB1Index;
    [SerializeField] protected int iWayPointsRandom;
    [SerializeField] protected Transform[] WayPointsRoute1;
    [SerializeField] protected Transform[] WayPointsRandomRoute;
    protected NavMeshAgent agent;
    protected enum EnemyMoveState_Enum
    { chasing, patrolling, AttackMove1, updating}
    [SerializeField] protected EnemyMoveState_Enum Move_State;
    protected EnemyMoveState_Enum enemyMoveState
    {
        get { return Move_State; }
        set {Move_State = value; } 
    } 
    #endregion

    [Header("Speed Settings")]
    #region SpeedSettings
    //SLOW SPEED
    [SerializeField] protected float MoveSpeedSlow;
 virtual protected float SpeedSlow
    {
        get { return MoveSpeedSlow; }
        set { MoveSpeedSlow = value; }
    }
    //NORMAL SPEED
    [SerializeField] protected float MoveSpeedNormal;
    virtual protected float SpeedNormal
    {
        get { return MoveSpeedNormal; }
        set {MoveSpeedNormal = value; }
    }
    //FAST SPEED
    [SerializeField]protected float MoveSpeedFast;
    virtual protected float SpeedFast
    {
        get { return MoveSpeedFast; }
        set { MoveSpeedFast = value; }
    }

    #endregion

    [Header("Combat Settings")]
    #region CombatSettings
    [SerializeField] protected Transform[] WayPoint_AttackRoute1;
    [SerializeField] protected int index_AttackWroute1;
    [SerializeField] protected bool isAttacking;
    
    #endregion

    #endregion

    #region RUNTIME
    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    protected virtual void Update()
    {
        DistanceToPlayer = Vector3.Distance(Player.position , transform.position);
        switch (Move_State)
        {
            case EnemyMoveState_Enum.chasing:
                HandleStateSwitches();
            break;
            case EnemyMoveState_Enum.patrolling:
                HandleStateSwitches();
            break;
            case EnemyMoveState_Enum.AttackMove1:
                HandleStateSwitches();
            break;
        }
    }
    #endregion

    #region METHODS&BOOLS
    #region StateMachine
    abstract protected void StateLogic(EnemyMoveState_Enum UpdateState);
    abstract protected void HandleStateSwitches();
    #endregion
    #region MovementLogic
    abstract protected void ChasingLogic();
    abstract protected void PatrollingLogic();
    abstract protected void PatrollingRandomLogic();

    #endregion
    #region NpcManagement
    abstract protected void SpawnLogic();
    #endregion
    #region COMBAT
    abstract protected IEnumerator AttackOne();
    abstract protected void AttackTwo();

    
    #endregion
    #endregion
}
