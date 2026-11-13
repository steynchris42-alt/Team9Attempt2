using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy_parent_Class : MonoBehaviour
{
    #region Settings
    //==Settings==//

    //-- Overrides in Child classes--//
    protected Coroutine State_Tracker_Og_Coro; // backing field to setup circle of life coroutine in this class
    public virtual Coroutine State_Tracker_Coro //Gets the PARENT coro and sets its value as an overridable variable
    {
    get => State_Tracker_Og_Coro;
    set => State_Tracker_Og_Coro = value;
    }

    //State_Tracker_Coro is overridable in child classes and retains the vlaue of State_Tracker_Og_Coro


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
    public Coroutine Respawning_Coro;

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
   private Enemy_HunterType_Child Enemy_Hunter;
  // public KillTracker KillTracker_scr;
    //--Silent reference
    public Enemy_parent_Class Enemy_Parent;
    #endregion 
    public void Awake()
    {
        if (!MeshRen || !collider || !agent || !Enemy_Reg ||!Enemy_Hunter || !particle)
        {
            MeshRen = GetComponent<MeshRenderer>();
            collider = GetComponent<Collider>();
            agent = GetComponentInParent<NavMeshAgent>();
            particle = GetComponentInChildren<ParticleSystem>();
            Enemy_Reg = GetComponent<Enemy_RegularType_Child>();
            Enemy_Hunter = GetComponent<Enemy_HunterType_Child>();
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
        Debug.DrawLine(transform.position, player.transform.position, color: Color.green);
    HealthTracker();
    }
    #endregion
    #region Life to death cycle
    protected virtual void HealthTracker() //---Determines when the enemy should die--//
    { 
        Mathf.Clamp(Enemy_HealthBar.value, Enemy_HealthBar.minValue, Enemy_HealthBar.maxValue);
        if (Enemy_HealthBar.value <= Enemy_HealthBar.minValue && Respawning_Coro == null && IsDead == true)
        {
            Die();
            Respawning_Coro = StartCoroutine(CircleOFLife());
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

      //  Enemy_Reg.Ranger_Anim.SetBool("isFlying",false); // starts FYLING animation
       // Enemy_Reg.Ranger_Anim.SetBool("isIdle", true); // stops idle animation  = false; //stops Ranger Flying animaton

        isPatrolling = false;

        if (State_Tracker_Coro != null)
           {
            State_Tracker_Coro = null; //OV
           } 
            MoveToSpawn();

         yield return new WaitForSeconds(5);

        Enemy_HealthBar.value = Enemy_HealthBar.maxValue;
        MeshRen.enabled = true;
        collider.enabled = true;
        IsDead = false;
        agent.isStopped = false;
        Respawning_Coro = null;
        }
            yield return null;
    }
    public virtual void TakeDamage()
    {
        Mathf.Clamp(Enemy_HealthBar.value -= 5, Enemy_HealthBar.minValue, Enemy_HealthBar.maxValue);
       // Enemy_Reg.Ranger_Anim.SetBool("isTakingDamage",true); //lays DAMAGE animation
        particle.Play();
       // Enemy_Reg.Ranger_Anim.SetBool("isTakingDamage", false); //Stops DAMAGE animation
    }
    public virtual void Take_Stab_Damage()
    {
        Mathf.Clamp(Enemy_HealthBar.value -=20, Enemy_HealthBar.minValue, Enemy_HealthBar.maxValue);
        // Enemy_Reg.Ranger_Anim.SetBool("isTakingDamage",true); //lays DAMAGE animation
        particle.Play();
        // Enemy_Reg.Ranger_Anim.SetBool("isTakingDamage", false); //Stops DAMAGE animation
        Debug.Log("AYyyy ive beenb SDTabbbed!!");
    }
    public void MoveToSpawn()
    {
        Debug.Log("Moving to spawn");
        if (IsAtSpawn == false)
        {
        Respawn_Index = UnityEngine.Random.Range(0, RespawnLocations.Length);
        transform.position = RespawnLocations[Respawn_Index].position;
        IsAtSpawn = true;
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
        else if (collision.gameObject.CompareTag("MurderBotSword"))
        {
            Debug.Log("SwordHit");
            Take_Stab_Damage();
        }
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