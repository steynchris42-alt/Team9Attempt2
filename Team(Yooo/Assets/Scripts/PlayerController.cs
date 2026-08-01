using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public InputAction move;
    
    //directional vectors for WASD movement
    protected Vector3 MoveForward = new Vector3 (0,0,1);
    protected Vector3 MoveBackward = new Vector3 (0,0,-1);
    protected Vector3 MoveLeft = new  Vector3(-1,0,0);
    protected Vector3 MoveRight = new Vector3(1,0,0);
    protected Vector3 Motion;

    //player speed settings
    protected float MoveSpeed = 5.0f;
    public void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    public void Update()
    {
        if (Motion != null)
        {
            PlayerMovement();
        }
    }
    public void PlayerMovement()
    {
     Motion = MoveForward + MoveBackward + MoveLeft + MoveRight * MoveSpeed * Time.deltaTime;
      
    controller.Move(Motion);

    }
    public void CameraController()
    {
       
    }
}
