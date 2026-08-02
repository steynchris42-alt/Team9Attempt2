using System;
using System.Data;
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
    [SerializeField] protected Transform[] WayPointsRoute1;
    //FOR STATE SWITCHING
    [SerializeField] protected bool IsStateSwitching;
    protected NavMeshAgent agent;
    protected enum EnemyMoveState_Enum
    { chasing, patrolling, updating}
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
        set { value = MoveSpeedNormal; }
    }
    //FAST SPEED
    [SerializeField]protected float MoveSpeedFast;
    virtual protected float SpeedFast
    {
        get { return MoveSpeedFast; }
        set { value = MoveSpeedFast; }
    }

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
        switch (DistanceToPlayer)
        {
            case <=100:
                Move_State = EnemyMoveState_Enum.chasing;
                ChasingLogic();
            break;
            case >= 100:
               Move_State = EnemyMoveState_Enum.patrolling ;
                PatrollingLogic();
            break;
        }
        MovementStateSwitch(Move_State);
    }
    #endregion

    #region METHODS&BOOLS
    #region MovementLogic
    abstract protected void MovementStateSwitch(EnemyMoveState_Enum UpdateState);
    abstract protected void ChasingLogic();
    abstract protected void PatrollingLogic();
    abstract protected bool IsPatrolling();
    abstract protected bool IsChasing();
    #endregion

    #region NpcManagement
    abstract protected void SpawnLogic();
    #endregion
   #endregion
}
