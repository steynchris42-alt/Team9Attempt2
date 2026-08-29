using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{

    public GameObject FirePoint;
    public Rigidbody RigBod;
    public ForceMode Bullet_Force;
    public float Bullet_Speed = 100.0f;
    public void Start()
    {
        RigBod = RigBod.GetComponent<Rigidbody>();
        Bullet_Force = ForceMode.Impulse;
    }
    public void Fire_Bullet()
    {
        if (RigBod != null)
        {
            RigBod.AddForce(transform.forward * Bullet_Speed, Bullet_Force);
        }
        Debug.Log("Bullet Fired Enemy");
    }
}
