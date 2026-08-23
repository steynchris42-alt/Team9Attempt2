using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
public class Shooting : MonoBehaviour
{
    [Header("Shooting Settints")]
    //CallBack context condition in PlayerControllerScript//
    public bool isShooting;
    //--//
    public GameObject FirePoint;
  public GameObject Bullet;
    public Bullet_Scr bullet_Script;
    public Coroutine Shoot_Cor;
    public void ShootingLogic()
    {

        if (isShooting == true && Shoot_Cor == null)
        {
            Shoot_Cor = StartCoroutine(BulletSpawnRate());
        }
        
    }
    public IEnumerator BulletSpawnRate()
    {
      //  isShooting = true;
        while (isShooting == true)
        {
            Spawn_Bullet();
          yield return new WaitForSeconds(0.5f);

        }
        Shoot_Cor = null;
        yield return new WaitForSeconds(0.5f);
       

        // isShooting=false;



    }
    public void Spawn_Bullet()
    {

        GameObject Active_Bullet = Instantiate(Bullet, FirePoint.transform.position, FirePoint.transform.rotation);
        bullet_Script = Active_Bullet.GetComponent<Bullet_Scr>();
        if (bullet_Script != null)
        {
            bullet_Script.Fire_Bullet();

            // yield return new WaitForSeconds(1);
            //isShooting = false;
        }

    }

}
