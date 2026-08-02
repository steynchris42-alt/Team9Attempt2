using UnityEngine;
using UnityEngine.AI;

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
     get { return iEnemyHeavyTypeTracker;}
     set { value = iEnemyHeavyTypeTracker; }
    }

    #endregion

    [Header ("Navmesh Settings")]
    #region NavmeshSettings
  //TARGETS
    [SerializeField] protected Transform Player;
    //FOR PATROLLING STATE
    [SerializeField] protected Transform Route1StartingTarget;
    [SerializeField] protected int iWaypointsB1Index;
    [SerializeField] protected Transform[] WayPointsRoute1;
    protected NavMeshAgent agent;
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
      if (agent != null)
        { 
            MovementStateSwitch(); 
        }
    }
    #endregion

   #region METHODS
    #region MovementLogic
    abstract protected void MovementStateSwitch();
    abstract protected void ChasingLogic();
    abstract protected void PatrollingLogic();
    #endregion

    #region NpcManagement
    abstract protected void SpawnLogic();
    #endregion
   #endregion
}
