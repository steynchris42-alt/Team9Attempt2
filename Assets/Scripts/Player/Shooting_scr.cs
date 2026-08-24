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
    
    //Method for the Player controlelr script
    public void ShootingLogic()
    {
        if (isShooting == true && Shoot_Cor == null)
        {
            Shoot_Cor = StartCoroutine(BulletSpawnRate());
        }   
    }

    // controls intervel between bullet shot.
    public IEnumerator BulletSpawnRate()
    {
        while (isShooting == true)
        {
            Spawn_Bullet();
          yield return new WaitForSeconds(0.2f);

        }
        Shoot_Cor = null;
        yield return new WaitForSeconds(0.2f);

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

}
