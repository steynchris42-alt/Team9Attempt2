using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_parent_Class : MonoBehaviour
{
    [Header("Movement settings")]
    protected float MoveSpeed_Defualt;
    protected float MoveSpeed_Fast;
    protected float MoveSpeed_Slow;

    [Header("Health and respawn Settings")]
   public int Max_health = 10;
   public int Min_health = 1;
    public int Current_health;

    //Spawning related
    [SerializeField] protected float RespawnTimer = 5.0f;
    protected float RespawnTimer_reset = 5.0f;


    private Collider collider;
    private MeshRenderer MeshRen;
    public Coroutine Respawning;

    public Transform[] RespawnLocations;
    public int Respawn_Index;

    [Header("Agent Pathfinding")]
    public GameObject player;
    protected float Dis_to_Player;
    [SerializeField] protected Transform[] Patrol_route_Random;
    [SerializeField] protected Transform[] WayPoint_AttackRoute1;
    [SerializeField]
    protected int Attackindex;
    [SerializeField] protected int Patrollindex;
    [SerializeField] protected Transform[] WayPoint_PatrollingRoute;

    [Header("NavMesh related")]
    protected NavMeshAgent agent;

    [Header("State Tracking")]
    public bool IsDead;
    public bool IsAtSpawn;
    [SerializeField] protected bool isPatrolling;
    [SerializeField] protected bool isChasing;
    public bool isSwitch_ToPatrolling;
    public bool isSwitch_ToChasing;
    public bool IsStateSwitch_Over;

    //References to otherScripts
    [Header("Other Scripts")]
   private Enemy_RegularType_Child Enemy_Reg;
   public KillTracker KillTracker_scr;
  

    public void Start()
    {
        Current_health = Max_health;
        MeshRen = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        Enemy_Reg = GetComponent<Enemy_RegularType_Child>();
    }
    #region Runtime
    public void Update()
    {
        Dis_to_Player = Vector3.Distance(transform.position , player.transform.position);
     

        HealthTracker();
        KillTracker_scr.IncreaseCount();
        if (Respawning == null && IsDead == true)
        {
            Respawning = StartCoroutine(CircleOFLife());
        }
    }
    #endregion
    #region Life to death cycle
    protected virtual void HealthTracker()
    { 
        Mathf.Clamp(Current_health, Min_health, Max_health);
        if (Current_health <= Min_health)
        {
            Die();

            //StartCoroutine(CircleOFLife());
        }
    }
    protected virtual void Die()
    {
        IsAtSpawn = false;
        IsDead = true;
       
        KillTracker_scr.isKill = true;
        KillTracker_scr.isKill = false;
    }
    protected IEnumerator CircleOFLife()
    {
     {
       while (IsDead)
         {
          yield return null;
          MeshRen.enabled = false;
          collider.enabled = false;
          agent.isStopped = true;
          isChasing = false;
          isPatrolling = false;
          Enemy_Reg.State_Tracker_Coro = null;
     
           MoveToSpawn();
           yield return new WaitForSeconds(5);
                Current_health = Max_health;
                MeshRen.enabled = true;
           collider.enabled = true;
           IsDead = false;
             
        agent.isStopped = false;
          Respawning = null;
            }
            yield return null;
        }
    }
    protected virtual void TakeDamage()
    {
        Current_health--;
    }
    public void MoveToSpawn()
    {
        if (IsAtSpawn == false)
        {
            Respawn_Index = Random.Range(0, RespawnLocations.Length);
            transform.position = RespawnLocations[Respawn_Index].position;
            IsAtSpawn = true;
        }
    }
    protected virtual void Spawn()
    {
        if (Enemy_Reg.gameObject.activeInHierarchy == false)
        {
            RespawnTimer -= Time.deltaTime;
        }
        if (RespawnTimer <= 0)
        {
            RespawnTimer = RespawnTimer_reset;
            Current_health = Max_health; 
        }
    }
    #endregion
    #region Movement Logic
    #endregion
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
          TakeDamage();
        }
    }
}
