using UnityEngine;
using System.Collections;

public class Enemy_Shoot_mech : MonoBehaviour
{

    //--//
    public GameObject FirePoint;
    public GameObject Bullet;

    public Enemy_Bullet Enemy_bullet_Script;
    public Enemy_RegularType_Child EnemyChild_Scr;

    public Coroutine Shoot_Cor;

    public void ShootingLogic()
    {
        if (Shoot_Cor == null && EnemyChild_Scr.isAttacking == true)
        {
            Shoot_Cor = StartCoroutine(BulletSpawnRate());
        }
    }

    // controls intervel between bullet shot.
    public IEnumerator BulletSpawnRate()
    {
        while (EnemyChild_Scr.isAttacking == true)
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
        Enemy_bullet_Script = Active_Bullet.GetComponent<Enemy_Bullet>();
        Destroy(Active_Bullet, 2.0f);
        if (Enemy_bullet_Script != null)
        {
            Enemy_bullet_Script.Fire_Bullet();

        }
    }
    


}


