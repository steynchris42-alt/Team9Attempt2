using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public ParticleSystem Enemy_bullet_Destroy_part;
    public GameObject FirePoint;
    public Rigidbody RigBod;
    public ForceMode Bullet_Force;
    public float Bullet_Speed = 1000.0f;
    public MeshRenderer meshRen;

    public bool isEnemyBullet_Colliding;
    public Enemy_Shoot_mech EnemShoot;
    public void Start()
    {
        RigBod = RigBod.GetComponent<Rigidbody>();
        meshRen =  meshRen.GetComponent<MeshRenderer>(); 
        

        
        Bullet_Force = ForceMode.Impulse;
    }
    public void Fire_Bullet()
    {
        if (RigBod != null)
        {
            RigBod.AddForce(transform.forward * Bullet_Speed, Bullet_Force);
        }
        Debug.Log("ENEMY BULLET FIRED");
    }
    public void OnCollisionEnter(Collision collision)
    {
      //  Debug.Log("ENEMY BULLET collision");
        meshRen.enabled = false;
         Enemy_bullet_Destroy_part.Play();
        Destroy(gameObject, 0.5f);
        
    }

}
