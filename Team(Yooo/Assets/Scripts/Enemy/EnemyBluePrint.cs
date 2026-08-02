using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBluePrint : MonoBehaviour
{
    [Header ("Enemy config")]

    #region INSTANCE FIELDS
   [Header ("Navmesh Settings")]
    #region NavmeshSettings
    protected NavMeshAgent agent;
    [SerializeField] protected Transform Player;
 #endregion
    [Header("Speed Settings")]
 #region SpeedSettingsSLOW
  [SerializeField] protected float MoveSpeedSlow;
 virtual protected float SpeedSlow
    {
        get { return MoveSpeedSlow; }
        set { MoveSpeedSlow = value; }
    }
    #endregion

 #region SpeedSettingsNORMAL
    [SerializeField] protected float MoveSpeedNormal;
    virtual protected float SpeedNormal
    {
        get { return MoveSpeedNormal; }
        set { value = MoveSpeedNormal; }
    }
    #endregion
 
 #region SpeedSettingsFAST
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
            ChasingLogic(); 
        }
    }
    #endregion

    #region METHODS
    #region MovementLogic
    abstract protected void ChasingLogic();
    abstract protected void PatrollingLogic();
    #endregion
  #endregion
}
