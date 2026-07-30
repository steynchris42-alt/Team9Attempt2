using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //inputStuff
    private Keyboard keyboard;
    [SerializeField] private Rigidbody Rigbod;

    //Camera Stuff
    [SerializeField] private float Camx;
    [SerializeField] private float Camy;
    [SerializeField] private float Camz;

    //physics stuff
    private float MoveSpeedBase = 10.0f ;
    public void Start()
    {
        keyboard = Keyboard.current;
        Rigbod = GetComponent<Rigidbody>();
    }
    public void Update()
    {
     
    }
    public void PlayerMovement()
    {
        Vector3 MoveDir = new Vector3(0, 0, 0);
        Vector3 MoveForce = MoveDir * Time.deltaTime * MoveSpeedBase;
        Rigbod.AddForce(MoveForce);
    }
    public void CameraController()
    {

    }
}
