using UnityEngine;
using System.Collections;

public class Enemy_Shoot_mech : MonoBehaviour
{
    public bool isShooting;
    //--//
    public GameObject FirePoint;
    public GameObject Bullet;

    public Enemy_Bullet Enemy_bullet_Script;
    public Enemy_RegularType_Child EnemyChild_Scr;

    public Coroutine Shoot_Cor;

    //Method for the Player controlelr script
    public void ShootingLogic()
    {
        if (Shoot_Cor == null)
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
        Debug.Log("bullet should fire");
        GameObject Active_Bullet = Instantiate(Bullet, FirePoint.transform.position, FirePoint.transform.rotation);
        Enemy_bullet_Script = Active_Bullet.GetComponent<Enemy_Bullet>();
        if (Enemy_bullet_Script != null)
        {
            Enemy_bullet_Script.Fire_Bullet();

        }

    }

}


