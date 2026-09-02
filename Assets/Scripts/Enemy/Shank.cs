using UnityEngine;


public class Shank : MonoBehaviour
{
    private Rigidbody Rigbod;

    private Vector3 Direction;
    private Vector3 Shank_Force;
    private Vector3 Shank_Force_Retreat;

    private float ShankSpeed = 20.0f;

    private float ShankAttack_Timer;

    [SerializeField] private Transform player;
    [SerializeField] private Transform Shankbase;
    public void Start()
    {
        Rigbod = GetComponent<Rigidbody>();
        ShankAttack_Timer = 3.0f;
    }
    
    public void shank_attack()
    {
        Shank_Force = (player.position - transform.position).normalized;
        Shank_Force_Retreat = (Shankbase.position - transform.position).normalized;

        Rigbod.AddForce(Shank_Force * ShankSpeed, ForceMode.Impulse);
        ShankAttack_Timer -= Time.deltaTime;
        if (ShankAttack_Timer <= 0)
        {
            Rigbod.AddForce(Shank_Force_Retreat * ShankSpeed, ForceMode.Impulse);
        }
    }
    public void Shank_Retreat()
    {
     
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SHANK ASHANK!");
        }
    }

}
