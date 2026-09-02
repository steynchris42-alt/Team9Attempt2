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
   [SerializeField] private Vector2 Movement_Vector;
   [SerializeField] private bool isSprinting;
   [SerializeField] private bool isJumping;
   [SerializeField] private bool isDashing;
   public Shooting shoot_script_ref;

    //--speed settings--//
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private float SprintSpeed = 10.0f;
   private float SpeedReset = 5.0f;

    //--Physics settings--//
    private float DownForce = -2f;

     private Vector3 Player_vert;
    private Vector3 Player_horo;
    private Vector3 Player_Forward;

    private Vector3 Move_Collab;
    Vector3 motion_Direction;

    [Header("Jump_Settings")]
    //--Jump settings--//
    [SerializeField] private float JumpForce = 5.0f;
    [SerializeField] private float JumpForce_down = -5.0f;

    [Header("Jump_Settings")]
    //--Dash settings--//
    [SerializeField] private float DashForce = 10.0f;
    


    [Header("CameraSTuff")]
    private Camera Playercam;
    protected Vector2 LookVector;
   private float LookSensitivity = 0.5f;
     private float CamX;
    private float CamY;
    private float VerticleRotation;
    private float HorozontalRotation;

    //to track controller.isgroudned
    [SerializeField] private bool isGround_bool;
    [Header("Coroutine related")]
    
    public Coroutine Mobility_coro;
    #endregion
    [SerializeField] private bool isDashing_Stop;
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
    public void Jump(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValueAsButton();
    }
    public void Dash(InputAction.CallbackContext context)
    {
        isDashing = context.ReadValueAsButton();
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
        else if (controller.isGrounded == false && isJumping == false ) 
        {
            Player_vert.y += Physics.gravity.y * Time.deltaTime; //physics epic yeye
        }
        Move_Collab = (motion_Direction * MoveSpeed) + new Vector3(0, Player_vert.y, 0) + new Vector3(0,0, Player_Forward.z);
        controller.Move(Move_Collab * Time.deltaTime);
        
        if (isJumping == true && Mobility_coro == null && controller.isGrounded == true)
        {
            Mobility_coro = StartCoroutine(Jumpyjump());
        }
        if (isDashing && Mobility_coro == null)
        {
            Mobility_coro = StartCoroutine(Dash_Logic());
        }

       
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
    public IEnumerator Jumpyjump()
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
    public IEnumerator Dash_Logic()
    { 
        while (isDashing == true && isDashing_Stop == false && Mobility_coro == null)
        {
            {
                Debug.Log("Dashhhh");
                Player_Forward.z = DashForce;
                 yield return new WaitForSeconds(0.1f);
                Player_Forward.z = 0.0f;
                isDashing_Stop = true;
            }
            yield return null;

            Mobility_coro = null;
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
    #endregion
}

