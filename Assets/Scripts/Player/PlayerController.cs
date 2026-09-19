using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;
using System.Collections;



public class PlayerController : MonoBehaviour
{
    #region INSTANCE FIELDS
    [Header ("MovementRelated")]

    [SerializeField] private CharacterController controller;
    //Input actions contexts

   [SerializeField] private bool isSprinting;
    [SerializeField] private bool isStopSprinting;
    [SerializeField] private bool isJumping;
   [SerializeField] private bool isDashing;
 
    public Shooting shoot_script_ref;

    //--speed settings--//
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private float SprintSpeed = 10.0f;
   private float SpeedReset = 5.0f;

    //--Physics settings--//
    private float DownForce = -2f;

     private Vector3 Player_vert; // This vector will refer to the players position on the Y axis (up and down)
    Vector3 motion_Direction;
    [SerializeField] private Vector2 Movement_Vector; // this is the condition for movemnet action map
    private Vector3 Move_Collab; //The final vector that will act as the parameter to move character controller

   
    private Vector3 DashMotion;
    private Vector3 Dash_Collab;
    private Vector3 Dash_Dir;

    [Header("Jump_Settings")]
    //--Jump settings--//
    [SerializeField] private float JumpForce = 5.0f;
    [SerializeField] private float JumpForce_down = -5.0f;

    [Header("Dash_Settings")]
    //--Dash settings--//
     private float DashForce = 100.0f;

   private float Dash_Event_Timer = 0f;
    private float Dash_EventEnd_Timer = 0.1f;
    public GameObject player;


    [Header("CameraSTuff")]
    private Camera Playercam;
    protected Vector2 LookVector;
   private float LookSensitivity = 0.5f;
     private float CamX;
    private float CamY;
    private float VerticleRotation;
    private float HorozontalRotation;

    private float Fov_Max = 90;
    private float Fov_Min = 80;

    //to track controller.isgroudned
    [SerializeField] private bool isGround_bool;
    [Header("Coroutine related")]
    
    public Coroutine Mobility_coro;
    public Coroutine Mobility_coro_two;
    #endregion
    [SerializeField] private bool isDashing_Stop;
    public float FOV_motion = 0.0f;
    #region RUNTIME
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
            isGround_bool = controller.isGrounded;
            MoveLogic();
            CameraLogic();
            shoot_script_ref.ShootingLogic();
    }
    #endregion

    #region INPUT ACTION CALLBACKS
    public void Movement(InputAction.CallbackContext context)
    {
        Movement_Vector = context.ReadValue<Vector2>();
   
    } 
   public void Sprint (InputAction.CallbackContext context)
    {
        if(context.performed && Mobility_coro == null)
        {
            isSprinting = true;
            Mobility_coro_two = StartCoroutine(Sprinty());
        }
        else if (context.canceled )
        {
            Debug.Log("stopping sprint context");
            isSprinting=false;
        }
    }
   public void PlayerLook(InputAction.CallbackContext context)
    {
        LookVector = context.ReadValue<Vector2>();
    }
   public void Shooting(InputAction.CallbackContext context)
    {
        shoot_script_ref.isShooting = context.ReadValueAsButton();
    }
    public void Jump(InputAction.CallbackContext context)
    {
       if (context.performed)
        {
            Mobility_coro = StartCoroutine(JumpLogic());
        }
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && Mobility_coro_two == null)
        {
            Dash_Dir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            isDashing = true;
         Mobility_coro_two = StartCoroutine(Dash_Logic());
     
            Debug.Log("Dash conetxt");
        }

    }
    #endregion

    #region LOGIC
    public void MoveLogic()
    {
        motion_Direction = Movement_Vector.x * transform.right + Movement_Vector.y * transform.forward ;
        
        if (controller.isGrounded  == true && Player_vert.y < 0 && isJumping == false)
        {
            Player_vert.y = DownForce;
        }
        else if (controller.isGrounded == false && isJumping == false) 
        {
            Player_vert.y += Physics.gravity.y * Time.deltaTime; //physics epic yeye
        }
        Move_Collab = (motion_Direction * MoveSpeed) + new Vector3(0, Player_vert.y, 0)   ;
        controller.Move(Move_Collab * Time.deltaTime);

    }

    public IEnumerator Sprinty()
    {
        float TimerStart = 0;
        float TimerEnd = 5;
       
            Debug.Log("Sprinting Start");
            MoveSpeed = SprintSpeed;
            Playercam.fieldOfView = Fov_Max;   

        while (isSprinting  && TimerStart<TimerEnd)
        {
            TimerStart += Time.deltaTime;
            yield return null;
        } //Abovge logic is executed ebery frame until conditions of the loop are no longer met

        MoveSpeed = SpeedReset;
        Playercam.fieldOfView = Fov_Min;
        Mobility_coro_two = null;

    }
    public IEnumerator JumpLogic()
    {
        if (Mobility_coro == null && controller.isGrounded == true)
        {
            Debug.Log("TO TEH SKIEEE");
            Player_vert.y = JumpForce;
            yield return new WaitForSeconds(0.5f);
            Player_vert.y = JumpForce_down;
            yield return new WaitUntil(() => controller.isGrounded == true);
            Mobility_coro = null;
        }
    }
    public IEnumerator Dash_Logic()
    {
        DashMotion = DashForce * Dash_Dir;
        while (isDashing)
        {
             if (Dash_Event_Timer < Dash_EventEnd_Timer)
                {
                   Playercam.fieldOfView = Fov_Max;
                  controller.Move(DashMotion * Time.deltaTime);
                Dash_Event_Timer += Time.deltaTime;
                    Debug.Log("Dashhhh");
                }
                else if (Dash_Event_Timer >= Dash_EventEnd_Timer)
                {
                    Playercam.fieldOfView = 80;
                    Dash_Event_Timer = 0.0f;
                    isDashing = false;
                Debug.Log("Dash stopped");
                }
                yield return null;
            }
        Mobility_coro_two = null;
    }

    #endregion
    #region CAMERA STUFF
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

    
    #endregion
}

///////////////////////////////////////////////////////////////////////////////////////////////////
///--------------ARCHIVE--------------------------/////////////
/*
 *                      ---THE OLD JUMP LOGIC---
 *     public IEnumerator Jumpyjump()
    {
        while (isJumping == true && controller.isGrounded == true)
        {
            {
                Debug.Log("jumpy jumpy");
                Player_vert.y = JumpForce;
                yield return new WaitForSeconds(1f);
                Player_vert.y = JumpForce_down;
            }
            yield return null;
            Mobility_coro = null;
        }        
    }
Simplified afetr I figured out how context.started works


                        ---THE OLD DASH LOGIC---
   
    public void DashCash()
    {
        if (isDashing)
        {
            Dash_Dir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            DashMotion = DashForce * Dash_Dir;
            if (Dash_Event_Timer < Dash_EventEnd_Timer)
            {
                controller.Move(DashMotion * Time.deltaTime);
                Dash_Event_Timer += Time.deltaTime;

            }
            else if (Dash_Event_Timer >= Dash_EventEnd_Timer)
            {
                Dash_Event_Timer = 0.0f;
                isDashing = false;
            }
         }
    }

 */