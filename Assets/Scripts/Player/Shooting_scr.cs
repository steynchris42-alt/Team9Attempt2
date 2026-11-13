using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
public class Shooting : MonoBehaviour
{
    [Header("Shooting Settints")]
    public bool isShooting; //CallBack context condition in PlayerControllerScript//
    public bool isRaycastHit;
    public Coroutine Shoot_Cor;
    public GameObject WhiteReticle;
    public GameObject RedReticle;
    public PlayerController playerController;

    public AudioSource Shooting_Audio;

    //--Raycast Information--//
    [Header("RAYCAST RELATED")]
    public Transform Ray_Origin;
    public Transform Ray_End;
    public LayerMask Enemy;
    public RaycastHit ShootHit;

    public GameObject BulletEffect_Parent;
    public void Awake()
    {
        isShooting = false;
        WhiteReticle.SetActive(true);
        RedReticle.SetActive(false);
        
    }
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
            Enemy_RegularType_Child EnemyReg = ShootHit.collider.GetComponent<Enemy_RegularType_Child>(); //ENEMY RANGER COMP
            Enemy_HunterType_Child EnemyHunter = ShootHit.collider.GetComponent<Enemy_HunterType_Child>(); //ENEMY HUNTER COMP
            if (EnemyReg != null )
                {
                Debug.DrawRay(RayOrigin, RayDir, Color.red);
                //WhiteReticle.SetActive(false);
                //RedReticle.SetActive(true);
                Debug.Log("ENEMY SHOOT HIT");
                EnemyReg.TakeDamage();
                }
            else if (EnemyHunter != null)
            {
                EnemyHunter.TakeDamage();
            }
 
            }
        else
        {
            Debug.DrawRay(RayOrigin, RayDir, Color.green);
            // WhiteReticle.SetActive(true);
            // RedReticle.SetActive(false);
            isRaycastHit = false;
            return;
        }

    }

    public IEnumerator Shooting_Raycast_Coro()
    {
        //START SHOOIING EFFECTS HERE
        playerController.Player_EnergyBullets_Type1.Play();
        playerController.MuzzleFlash_part.Play();
        Shooting_Audio.Play();
        BulletEffect_Parent.SetActive(true);
        while (isShooting == true)
        {
           Shooting_Raycast();
            yield return new WaitForSeconds(0.1f);
        }
        //STOP SHOOIING EFFECTS HERE
        playerController.MuzzleFlash_part.Stop();
        playerController.Player_EnergyBullets_Type1.Stop();
        Shooting_Audio.Stop();
        //RESET SHOOIING EFFECTS HERE
        playerController.MuzzleFlash_part.Clear();
        playerController.Player_EnergyBullets_Type1.Clear();
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