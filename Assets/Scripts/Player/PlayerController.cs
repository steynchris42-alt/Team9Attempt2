using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;


public class PlayerController : MonoBehaviour
{
    [Header ("MovementRelated")]
    public CharacterController controller;
    //Input actions contexts
   protected Vector2 Movement_Vector;
   public bool isSprinting;
    public bool isShooting;
    //player speed settings
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private float SprintSpeed = 10.0f;
    [SerializeField] private float SpeedReset = 5.0f;


    //Directional vectors
    Vector3 Right = Vector3.right;
    Vector3 motion_Direction;

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
        ShootingLogic();
    }
    //--Callback Contexts--//
  public void Movement(InputAction.CallbackContext context)
    {
        Movement_Vector = context.ReadValue<Vector2>();
    } 
   public void Sprint (InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }
   public void PlayerLook(InputAction.CallbackContext context)
    {
        LookVector = context.ReadValue<Vector2>();
    }
   public void Shooting(InputAction.CallbackContext context)
    {
        isShooting = context.ReadValueAsButton();
    }
    //--Action Logic--//
    public void MoveLogic()
    {
        motion_Direction = Movement_Vector.x * transform.right + Movement_Vector.y * transform.forward ;
         controller.Move(motion_Direction * MoveSpeed * Time.deltaTime);
        SprintLogic();       
    }
    public void SprintLogic()
    {
        if (isSprinting)
        {
            MoveSpeed = SprintSpeed;
        }
        else
        {
            MoveSpeed = SpeedReset; 
        }
    }
    public void ShootingLogic()
    {
        if (isShooting)
        {
            Debug.Log("PewPew!");
        }
    }
    public void CameraLogic()
    {
        CamX = LookVector.x * LookSensitivity;
        CamY = LookVector.y * LookSensitivity;
        VerticleRotation = Mathf.Clamp(VerticleRotation, -90, 90);
       VerticleRotation -= CamY;
       HorozontalRotation -= -CamX;

       Playercam.transform.localRotation = Quaternion.Euler(VerticleRotation, 0,0);
        transform.Rotate(Vector3.up * CamX);
        Debug.Log("CameraLogicCalled");
    }
  
    public void GroundCheack()
    {
        Physics.Raycast(transform.position, Vector3.down, RayDis);
    }
}
