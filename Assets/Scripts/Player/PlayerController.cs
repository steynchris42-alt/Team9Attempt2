using JetBrains.Annotations;
using System.Collections;
using System.Net.Mime;
using TMPro;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region INSTANCE FIELDS
    [Header("UGUI RELATED")]
    //SPRINTING CANVAS//    
    public Slider Sprint_Slider;
    private float Sprint_Duration;
    public bool isSprintRecharging;

    //INTERACTION CANVAS// 
    public int iPowerUp_Type1;
    public int iPowerUp_Type2;
    public int iPowerUp_Type3;

    public TextMeshProUGUI PowerUp_UI_Type1;
    public TextMeshProUGUI PowerUp_UI_Type2;
    public TextMeshProUGUI PowerUp_UI_Type3;

    //raycast//
    public LayerMask Ground;
    public LayerMask interactable;
    public LayerMask House_Platform;

    [Header("MovementRelated")]
    [SerializeField] private CharacterController controller;
    //Input actions contexts
    [SerializeField] private bool isSprinting; //returns true based on the timer in 'sprinty' Ienumerator
    [SerializeField] private bool isStopSprinting;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isInteracting;
    [SerializeField] private bool IsUsing_PowerUp_Type1;



    public bool isAbleToInteract;//Set in "Interactable_new" script

    public bool isMoving;
    public Shooting shoot_script_ref;


    //--speed settings--//
    [SerializeField] private float MoveSpeed = 7.0f;
    public float SprintSpeed = 8.0f;
    private float SpeedReset = 7.0f;

    //--Physics settings--//
    private float DownForce = -4f;
    private Vector3 Player_vert; // This vector will refer to the players position on the Y axis (up and down)
    Vector3 motion_Direction;
    [SerializeField] private Vector2 Movement_Vector; // this is the condition for movemnet action map
    private Vector3 Move_Collab; //The final vector that will act as the parameter to move character controller

    private Vector3 DashMotion;
    private Vector3 Dash_Collab;
    private Vector3 Dash_Dir;

    [Header("Jump_Settings")]
    //--Jump settings--//
    private float JumpForce = 1.0f;
    [SerializeField] private float JumpHeight = 1.0f;
    private float JumpForce_down = -10.0f;

    [Header("Dash_Settings")]
    //--Dash settings--//
    private float DashForce = 100.0f;

    private float Dash_Event_Timer = 0f;
    private float Dash_EventEnd_Timer = 0.1f;
    public GameObject player;

    [Header("CameraSTuff")]
    // public Camera Playercam;
    public CinemachineCamera Cin_cam;
    protected Vector2 LookVector;
    private float Fov_Max = 80;
    private float Fov_Min = 75;
    [SerializeField] private float MouseSenseX;
    [SerializeField] private float MouseSenseY;

    //to track controller.isgroudned
    [SerializeField] private bool isGround_bool;
    public bool IsRayGRound;
    [Header("Coroutine related")]
    //---Coroutines---//
    public Coroutine Mobility_coro;
    public Coroutine Mobility_coro_two;
    public Coroutine Camera_Shake_Coro;

    //---Particle effect related--//
    public ParticleSystem MuzzleFlash_part; //enabled in shooting script

    [SerializeField] private bool isDashing_Stop;
    public float FOV_motion = 0.0f;

    //---Camera Shake---/
    float Sine_Speed = 5.0f; //Controls interpolation speed
    float Sine_Mag = 0.5f; //controls magnitude of the sine wave

    [Header("OTHER SCRIPTS")]
 
    public LineOfSight_Scr LineOfSight_scr;

    #endregion

    #region RUNTIME
    public void Awake()
    {
        if (Cin_cam == null) TryGetComponent(out Cin_cam);
        if (controller == null) TryGetComponent(out controller);
        if (MuzzleFlash_part == null) GetComponentInChildren<ParticleSystem>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        isGround_bool = controller.isGrounded;
        GroundCheack();
        MoveLogic();
        shoot_script_ref.ShootingLogic();
        SprintCoolDown();  //Activates the cooldown for sprinting when the stamina bar is drained 
    }
    public void LateUpdate()
    {
        Cin_Camera_Logic();
    }
    #endregion

    #region INPUT ACTION CALLBACKS
    public void Movement(InputAction.CallbackContext context)
    {
        Movement_Vector = context.ReadValue<Vector2>();
        if (context.performed && Camera_Shake_Coro == null)
        {
            isMoving = true;
            Camera_Shake_Coro ??= StartCoroutine(Cin_Camera_Effects());
        }
        else if (context.canceled && Camera_Shake_Coro != null)
        {
            isMoving = false;
            Camera_Shake_Coro = null;
        }

    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed && Mobility_coro == null)
        {
            isSprinting = true;
            Mobility_coro_two = StartCoroutine(Sprinty());
        }
        else if (context.canceled)
        {
            Debug.Log("stopping sprint context");
            isSprinting = false;
        }
    }
    public void PlayerLook(InputAction.CallbackContext context)
    {

        //  LookVector = context.ReadValue<Vector2>();
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
    public void Interaction(InputAction.CallbackContext context)
    {  
        isInteracting = context.ReadValueAsButton();
        if (context.performed)
        {
            Debug.Log("Pressed E");
            if (LineOfSight_scr.isShowing_InteractPrompt_Notes) //condition for NOTES INTERACT PROMPT
            {
                LineOfSight_scr.isInteraction_Success_Notes = true; //enables NOTES

            }
            else if (LineOfSight_scr.isInteraction_Success_PowerUps_Type1) //condition for POWERUPS INTERACT PROMPT TYPE1
            {
               LineOfSight_scr.LineOfSight_PowerUp_Current.PowerUp_Purchase_(); //enables POWERUPS if the player has enough kills
               LineOfSight_scr.isInteraction_Success_PowerUps_Type1 = true;
                
            }
            else if (LineOfSight_scr.isInteraction_Success_PowerUps_Type2) //condition for POWERUPS INTERACT PROMPT TYPE2
            {
                LineOfSight_scr.LineOfSight_PowerUp_Current.PowerUp_Purchase_(); //enables POWERUPS if the player has enough kills
                LineOfSight_scr.isInteraction_Success_PowerUps_Type2 = true;
            }
            else if (LineOfSight_scr.isInteraction_Success_PowerUps_Type3) //condition for POWERUPS INTERACT PROMPT TYPE3
            {
                LineOfSight_scr.LineOfSight_PowerUp_Current.PowerUp_Purchase_(); //enables POWERUPS if the player has enough kills
                LineOfSight_scr.isInteraction_Success_PowerUps_Type3 = true;
            }
        }  
    }
     public void PowerUp_Type1(InputAction.CallbackContext context)
    {
        if (context.performed && iPowerUp_Type1 >= 1)
        {
            Debug.Log("Powerup type 1 USED");            
            iPowerUp_Type1 -= 1;
            PowerUp_UI_Type1.text = iPowerUp_Type1.ToString();
            // LineOfSight_scr.LineOfSight_PowerUp_Current.PowerUp_UI_Type1.text = iPowerUp_Type1.ToString();
            // IsUsing_PowerUp_Type1 = true;  
        }
        else if (context.performed && iPowerUp_Type1 < 1)
        {
            Debug.Log("Cannot use PowerUp TYPE!");

            // IsUsing_PowerUp_Type1 = true;  
        }
    }
    public void PowerUp_Type2(InputAction.CallbackContext context)
    {
        if (context.performed && iPowerUp_Type2 >= 1)
        {
            Debug.Log("Powerup type 1 USED");
           iPowerUp_Type2 -= 1;
            PowerUp_UI_Type2.text = iPowerUp_Type2.ToString();
            //LineOfSight_scr.LineOfSight_PowerUp_Current.PowerUp_UI_Type2.text = iPowerUp_Type2.ToString();
        }
        else if (context.performed && iPowerUp_Type2 < 1)
        {
            Debug.Log("Cannot use PowerUp TYPE!");

        }
    }
    public void PowerUp_Type3(InputAction.CallbackContext context)
    {
        if (context.performed && iPowerUp_Type3 >= 1)
        {
            Debug.Log("Powerup type 1 USED");
            iPowerUp_Type3 -= 1;
            PowerUp_UI_Type3.text = iPowerUp_Type3.ToString();
            //LineOfSight_scr.LineOfSight_PowerUp_Current.PowerUp_UI_Type3.text =     iPowerUp_Type3.ToString();

        }
        else if (context.performed && iPowerUp_Type3 < 1)
        {
            Debug.Log("Cannot use PowerUp TYPE!");

        }
    }
    #endregion

    #region LOGIC
    public void MoveLogic()
    {
        motion_Direction = Movement_Vector.x * transform.right + Movement_Vector.y * transform.forward;
        if (IsRayGRound == true && Player_vert.y < 0 && isJumping == false)
        {
            Player_vert.y = DownForce;
        }
        else if (IsRayGRound == false && isJumping == false)
        {
            Player_vert.y += Physics.gravity.y * Time.deltaTime; //physics epic yeye
        }
        Move_Collab = (motion_Direction * MoveSpeed) + new Vector3(0, Player_vert.y, 0);
        controller.Move(Move_Collab * Time.deltaTime);
    }

    public IEnumerator Sprinty()
    {
        float SliderTime = 0;
        float SliderDuration = 5.0f;
        float TimerStart = 0;
        float TimerEnd = 5;

        Debug.Log("Sprinting Start");

        MoveSpeed = SprintSpeed;
        while (isSprinting && TimerStart < TimerEnd && Sprint_Slider.value > Sprint_Slider.minValue) //This while loop makes the stamina bar move up when sprint key is being pressed and the bar is not empty
        {
            isSprintRecharging = false; 
            Sprint_Duration = 0;
            //above two lines reset the the sprinting cooldown in UPDATE
            TimerStart += Time.deltaTime;
            SliderTime += Time.deltaTime; // increases values over real seconds.
                                          // 
            Sprint_Slider.value = Mathf.MoveTowards(Sprint_Slider.value, Sprint_Slider.minValue, 3 * Time.deltaTime); //Decreases teh sprint value at a fixed time
            Cin_cam.Lens.FieldOfView = Fov_Max;
            yield return null;

            Sine_Speed = 10.0f; //increases camera swaying effect speed

        } //Abovge logic is executed ebery frame until conditions of the loop are no longer met
        isSprintRecharging = true;
        MoveSpeed = SpeedReset;
        Cin_cam.Lens.FieldOfView = Fov_Min;
        Sine_Speed = 5.0f;
        // Sprint_Slider.value = Mathf.Lerp(Sprint_Slider.minValue, Sprint_Slider.maxValue, SliderTime / SliderDuration);
        Mobility_coro_two = null;
    }
    public IEnumerator JumpLogic()
    {
        if (Mobility_coro == null && IsRayGRound == true)
        {
            Debug.Log("TO TEH SKIEEE");
            Player_vert.y =(JumpForce + JumpHeight) * 2 ;
            yield return new WaitForSeconds(0.2f) ;
            Player_vert.y = JumpForce_down;
           // Player_vert.y = Mathf.Sin(JumpForce_down * 8  *Time.time);
            yield return new WaitUntil(() => IsRayGRound == true);
            Mobility_coro = null;
        }
    }
    public void GroundCheack()
    {
        float RayDis = 2f;
        RaycastHit GroundRay;
        bool IsRayHit = Physics.Raycast(transform.position, Vector3.down, out GroundRay, RayDis, Ground + House_Platform);
        if (IsRayHit)
        {
            IsRayGRound = true;
            Debug.DrawRay(transform.position, Vector3.down * RayDis, Color.green);
        }
        else
        {
            IsRayGRound = false;
            Debug.DrawRay(transform.position, Vector3.down * RayDis, Color.red);
        }
    }
    public IEnumerator Dash_Logic()
    {
        DashMotion = DashForce * Dash_Dir;
        while (isDashing)
        {
            if (Dash_Event_Timer < Dash_EventEnd_Timer)
            {
                // Playercam.fieldOfView = Fov_Max;
                controller.Move(DashMotion * Time.deltaTime);
                Dash_Event_Timer += Time.deltaTime;
                Debug.Log("Dashhhh");
            }
            else if (Dash_Event_Timer >= Dash_EventEnd_Timer)
            {
                //Playercam.fieldOfView = 80;
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

    public void Cin_Camera_Logic()
    {
        float CinCamY = Cin_cam.transform.eulerAngles.y;
        quaternion JoinedRotation = Quaternion.Euler(0, CinCamY, 0);
        transform.rotation = JoinedRotation;
    }
    public IEnumerator Cin_Camera_Effects()
    {
        while (isMoving && Cin_cam != null)
        {
          Cin_cam.Lens.Dutch = Mathf.Sin(Time.time * Sine_Speed) * Sine_Mag;
          yield return null;
        }
        if (Cin_cam != null)
        {
          Cin_cam.Lens.Dutch = 0.0f;
        }
        Camera_Shake_Coro = null;
        #endregion
    }

    public void SprintCoolDown()
    {
      if (isSprintRecharging == true)
       {
        Sprint_Slider.value = Mathf.MoveTowards(Sprint_Slider.value, Sprint_Slider.maxValue, 1f * Time.deltaTime);
            Debug.Log("Recharging sprint");
       }
    }

  
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

                    ---Garbage Camera logic---

    public IEnumerator Camera_Movement_Response()
    {
        SmoothStep_Timer_start = 0.0f;
        while (isMoving)
        {
            t = Mathf.Clamp(SmoothStep_Timer_start / SmoothStep_Duration, 0, 1);
            if (isRotating_Right)
            {
                Cam_RotZ = Mathf.SmoothStep(0, -1, t);

                Debug.Log("Timer go UPP");
                SmoothStep_Timer_start += Time.deltaTime;

                if (SmoothStep_Timer_start >= SmoothStep_Timer_end)
                {
                    isRotating_Right = false;
                    //SmoothStep_Timer_start = SmoothStep_Timer_end;
                }
            }
            else if (!isRotating_Right)
            {
                Debug.Log("Timer go down");
                SmoothStep_Timer_start -= Time.deltaTime;
                if (SmoothStep_Timer_start <= 0.0f)
                {
                    isRotating_Right = true;
                    // SmoothStep_Timer_start = 0.0f;
                }
            }
            yield return null;

        }
        Cam_RotZ = 0;
        Camera_Shake_Coro = null;

                            ----THE OLD CAMERA LOGIC----
NOTE: Code works fine, it was for teh built in unity camera
    public void CameraLogic()
    {
        CamX = LookVector.x * LookSensitivity;
        CamY = LookVector.y * LookSensitivity;
        VerticleRotation = Mathf.Clamp(VerticleRotation, -90, 90);
        VerticleRotation -= CamY;
        HorozontalRotation -= -CamX;

        Cin_cam.transform.localRotation = Quaternion.Euler(VerticleRotation, 0, 0);
        transform.Rotate(Vector3.up * CamX);
        Debug.Log("CameraLogicCalled");
    }
    }

 */