using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy_parent_Class : MonoBehaviour
{
    [Header("Movement settings")]
    protected float MoveSpeed_Defualt;
    protected float MoveSpeed_Fast;
    protected float MoveSpeed_Slow;

    [Header("Health and respawn Settings")]
   public int Max_health = 50;
   public int Min_health = 1;
    public int Current_health;
    public Slider Enemy_HealthBar;
    //Spawning related
    [SerializeField] protected float RespawnTimer = 10.0f;
    protected float RespawnTimer_reset = 10.0f;

    public static Action Kill_Event; //utilised in Kill tracker script

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

    [Header("Particles and Effects")]
    private ParticleSystem particle;

    //References to otherScripts
    [Header("Other Scripts")]
   private Enemy_RegularType_Child Enemy_Reg;
   public KillTracker KillTracker_scr;
    //--Silent reference
    public Enemy_parent_Class Enemy_Parent;
    
    public void Awake()
    {
        if (!MeshRen || !collider || !agent || !Enemy_Reg || !particle)
        {
            MeshRen = GetComponent<MeshRenderer>();
            collider = GetComponent<Collider>();
            agent = GetComponent<NavMeshAgent>();
            Enemy_Reg = GetComponent<Enemy_RegularType_Child>();
            particle = GetComponentInChildren<ParticleSystem>();
        }
    }
    public void Start()
    {
        Current_health = Max_health;
    }
    #region Runtime
    public void Update()
    {
        Dis_to_Player = Vector3.Distance(transform.position , player.transform.position);
        HealthTracker();
        if (Respawning == null && IsDead == true)
        {
            Respawning = StartCoroutine(CircleOFLife());
        }
    }
    #endregion
    #region Life to death cycle
    protected virtual void HealthTracker() //---Determines when the enemy should die--//
    { 
        Mathf.Clamp(Enemy_HealthBar.value, Enemy_HealthBar.minValue, Enemy_HealthBar.maxValue);
        if (Enemy_HealthBar.value <= Enemy_HealthBar.minValue)
        {
            Die();
            StartCoroutine(CircleOFLife());
        }
    }
    protected virtual void Die()
    {
        if (IsDead == true) 
        {return;}
        IsAtSpawn = false;
        IsDead = true;

        Kill_Event.Invoke(); //TRIGGERS KILL TRACKER UPDATE
    }
    protected IEnumerator CircleOFLife() //--- CONTROLS THE WHOLE LIFE DEATH CYCLE.
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
                Enemy_HealthBar.value = Enemy_HealthBar.maxValue;
                MeshRen.enabled = true;
           collider.enabled = true;
           IsDead = false;
             
        agent.isStopped = false;
          Respawning = null;
            }
            yield return null;
        
    }
    public virtual void TakeDamage()
    {
        Mathf.Clamp(Enemy_HealthBar.value -= 5, Enemy_HealthBar.minValue, Enemy_HealthBar.maxValue);
        particle.Play();
    }
    public void MoveToSpawn()
    {
        if (IsAtSpawn == false)
        {
            Respawn_Index = UnityEngine.Random.Range(0, RespawnLocations.Length);
            transform.position = RespawnLocations[Respawn_Index].position;
            IsAtSpawn = true;
        }
    }
   /* protected virtual void Spawn() 
    {
        if (Enemy_Reg.gameObject.activeInHierarchy == false)
        {
            RespawnTimer -= Time.deltaTime;
        }
        if (RespawnTimer <= 0)
        {
            RespawnTimer = RespawnTimer_reset;
            Enemy_HealthBar.value = Enemy_HealthBar.maxValue;
        }
    }*/
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
