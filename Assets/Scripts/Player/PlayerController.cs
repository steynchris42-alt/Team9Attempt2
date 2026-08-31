using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;



public class PlayerController : MonoBehaviour
{
    [Header ("MovementRelated")]
    [SerializeField] private CharacterController controller;
    //Input actions contexts
   [SerializeField] private Vector2 Movement_Vector;
    [SerializeField] private bool isSprinting;
 
    //player speed settings
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private float SprintSpeed = 10.0f;
    [SerializeField] private float SpeedReset = 5.0f;
 //reference to shooting script
   public Shooting shoot_script_ref;
    //Directional vectors
    Vector3 Right = Vector3.right;
    Vector3 motion_Direction;

    [Header("Jump_Settings")]
    private Rigidbody RigBod;

    private bool isJumping;
    //raycast setup
    public LayerMask Ground;
    public GameObject TheFloor;
    public bool IsGrounded;

    private float DownForce = -2f;
    private Vector3 Player_vert;
    private Vector3 Move_Collab;

    [Header("CameraSTuff")]
    private Camera Playercam;
    protected Vector2 LookVector;
   private float LookSensitivity = 0.5f;
     private float CamX;
    private float CamY;
    private float VerticleRotation;
    private float HorozontalRotation;

    //RayCasting
    [SerializeField] private float RayDis = 5.0f;
   
    public void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    

        if (Playercam == null)
        {
            Playercam = Camera.main;
        }
    }
    public void Update()
    {
       
        MoveLogic();
        CameraLogic();
        //GroundCheack();
       shoot_script_ref.ShootingLogic();
        
        //GroundCheack();
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
        shoot_script_ref.isShooting = context.ReadValueAsButton();
    }
    public void Jumping(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValueAsButton();
    }
    //--Action Logic--//
    public void MoveLogic()
    {
        motion_Direction = Movement_Vector.x * transform.right + Movement_Vector.y * transform.forward ;
        if (controller.isGrounded  == true && Player_vert.y < 0 )
        {
            Player_vert.y = DownForce;
        }
        else
        {
            Player_vert.y += Physics.gravity.y * Time.deltaTime; //physics epic yeye
        }
        Move_Collab = (motion_Direction * MoveSpeed) + new Vector3(0, Player_vert.y, 0);
        controller.Move(Move_Collab * Time.deltaTime);


        
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
    public void JumpingLogic()
    {
   
        
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
  
    }

