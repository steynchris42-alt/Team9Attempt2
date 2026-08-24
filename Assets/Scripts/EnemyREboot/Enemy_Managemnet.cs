using System.Collections;
using UnityEngine;

public class Enemy_Managemnet : MonoBehaviour
{
   public Enemy_parent_Class EnemyParent;
  private Enemy_RegularType_Child EnemyReg;
   
   public Coroutine Respawning;
    public float respawn_timer = 5.0f;

   private MeshRenderer Mesh_Ren;
   private Collider collider;

    public void Start()
    {
        EnemyReg = GetComponent<Enemy_RegularType_Child>();
        Mesh_Ren = GetComponent<MeshRenderer>();
       collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    public void Update()
    {
        if ( Respawning == null && EnemyParent.IsDead == true)
        {
            Respawning = StartCoroutine(RespawnEnemy());
        }
    }
    public IEnumerator RespawnEnemy()
        {
        while (EnemyReg.IsDead)
        {
         
            yield return null;
            Mesh_Ren.enabled = false;
            collider.enabled = false;
            yield return new WaitForSeconds(respawn_timer);
            EnemyReg.MoveToSpawn();
            /* EnemyReg.Respawn_Index = Random.Range(0, EnemyReg.RespawnLocations.Length);
             transform.position = EnemyReg.RespawnLocations[EnemyReg.Respawn_Index].position; */
            EnemyReg.Current_health = EnemyReg.Max_health;
            Mesh_Ren.enabled = true;
            collider.enabled = true;
            EnemyReg.IsDead = false;
            Respawning = null;
        }
        yield return null;
        }
}