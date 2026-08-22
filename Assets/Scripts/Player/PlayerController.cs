using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;


public class PlayerController : MonoBehaviour
{
    [Header ("MovementRelated")]
    public CharacterController controller;
    //Input actions
    protected Vector2 Movement_Vector;
    //player speed settings
    [SerializeField] protected float MoveSpeed = 5.0f;
    //Directional vectors
  
    Vector3 Right = Vector3.right;
    Vector3 motion;

    [Header("CameraSTuff")]
    private Camera Playercam;
    protected Vector2 LookVector;
    protected float LookSensitivity = 0.5f;
    protected float CamX;
    protected float CamY;
    protected float VerticleRotation;
    protected float HorozontalRotation;

    //RayCasting
    protected float RayDis = 5.0f;
   
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        if (Playercam == null)
        {
            Playercam = Camera.main;
        }
    }
    public void Update()
    {
        MoveLogic();
        CameraLogic();
    }
    
  public void Movement(InputAction.CallbackContext context)
    {
        Movement_Vector = context.ReadValue<Vector2>();
    }
    public void MoveLogic()
    {
        motion = Movement_Vector.x * transform.right + Movement_Vector.y * transform.forward ;
        CollisionFlags collisionFlags = controller.Move(motion * MoveSpeed * Time.deltaTime);
        //Debug.Log("movementLogicCalled");
    }
    public void CameraLogic()
    {
        CamX = LookVector.x * LookSensitivity;
        CamY = LookVector.y * LookSensitivity;

       VerticleRotation -= CamY;
       HorozontalRotation -= -CamX;

       Playercam.transform.localRotation = Quaternion.Euler(VerticleRotation, 0, 0);
        transform.Rotate(Vector3.up * CamX);
        Debug.Log("CameraLogicCalled");
    }
    public void PlayerLook(InputAction.CallbackContext context)
    {
        LookVector = context.ReadValue<Vector2>();
    }
    public void GroundCheack()
    {
        Physics.Raycast(transform.position, Vector3.down, RayDis);
    }
}
