using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
public class Shooting : MonoBehaviour
{
    [Header("Shooting Settints")]
    public bool isShooting; //CallBack context condition in PlayerControllerScript//
    public bool isRaycastHit;
    public Coroutine Shoot_Cor;

    //External scripts
   public PlayerController playerController;
    //--Raycast Information--//
    [Header("RAYCAST RELATED")]
    public Transform Ray_Origin;
    public Transform Ray_End;
    public LayerMask Enemy;
    public RaycastHit ShootHit;

    public GameObject BulletEffect_Parent;
    public void ShootingLogic()
    {
  
        if (isShooting == true && Shoot_Cor == null)
        {
            Shoot_Cor = StartCoroutine(Shooting_Raycast_Coro());

        }   
    }   
    public void Shooting_Raycast()
    {
        Debug.Log("SHOTS FIRED!");
        Vector3 RayOrigin = Ray_Origin.position;
        Vector3 RayEnd = Ray_End.position;

        float RayDis =  Vector3.Distance(Ray_Origin.position, Ray_End.position);
        Vector3 RayDir = transform.forward;   //Direction of cast

        bool isShooting_Hit = Physics.Raycast(RayOrigin, RayDir, out ShootHit, RayDis, Enemy);
        isRaycastHit = isShooting_Hit;
        if (isShooting_Hit)
        {
            Enemy_RegularType_Child EnemyReg = ShootHit.collider.GetComponent<Enemy_RegularType_Child>();
               if (EnemyReg != null)
                {
                    Debug.DrawRay(RayOrigin, RayDir, Color.crimson);
                   //===== DEAL ONE DAMAGE TO ENEMY
                    Debug.Log("ENEMY SHOOT HIT");
                EnemyReg.TakeDamage();
                }
                else  {
                    Debug.DrawRay(RayOrigin, RayDir, Color.cyan);
                    isRaycastHit = false;
                    return; }
            }
        }

    public IEnumerator Shooting_Raycast_Coro()
    {
        playerController.MuzzleFlash_part.Play();
        BulletEffect_Parent.SetActive(true);
        while (isShooting == true)
        {
           Shooting_Raycast();
            yield return new WaitForSeconds(0.1f);
            playerController.MuzzleFlash_part.Stop();
        }
        playerController.MuzzleFlash_part.Clear();
        BulletEffect_Parent.SetActive(false);
        Shoot_Cor = null;
    }

}
//===============ARCHIVE========//
/*  ------THE OLD SHOOTING MECHANICS----------
////NOTE////
//Scraped because using a raycast for lign of sigh and particles for bullets is more accurate and less painfull.

    // controls intervel between bullet shot.
    public IEnumerator BulletSpawnRate()
    {
        while (isShooting == true)
        {
            Spawn_Bullet();
            playerController.MuzzleFlash_part.Play();
          yield return new WaitForSeconds(0.2f);

        }
        playerController.MuzzleFlash_part.Clear();
        playerController.MuzzleFlash_part.Stop();
        Shoot_Cor = null;
        //yield return new WaitForSeconds(0.2f);

    }
    //Instantiatea Active_bullet using 'Bullet' from the bullet script and runs the bullet travel logic
    public void Spawn_Bullet()
    {
        GameObject Active_Bullet = Instantiate(Bullet, FirePoint.transform.position, FirePoint.transform.rotation);
        bullet_Script = Active_Bullet.GetComponent<Bullet_Scr>();
        
        if (bullet_Script != null)
        {
            bullet_Script.Fire_Bullet();

        }

    }
 */