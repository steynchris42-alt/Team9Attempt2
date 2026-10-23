using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.Controls;
using System.Collections;

public class Player_Controller_Child : MonoBehaviour
{
    #region INSTANCE FIELDS
    [Header("Movement Related")]
    [SerializeField] private CharacterController controller;
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
    private Vector3 motion_Direction;
    private Vector3 Move_Collab;

    private Vector3 DashMotion;
    private Vector3 Dash_Dir;

    [Header("Jump Settings")]
    [SerializeField] private float JumpForce = 5.0f;
    [SerializeField] private float JumpForce_down = -5.0f;

    [Header("Dash Settings")]
    [SerializeField] private float DashForce = 30.0f;
    [SerializeField] private float DashDuration = 0.2f;

    [Header("Camera Stuff")]
    [SerializeField] private Camera Playercam;
    protected Vector2 LookVector;
    [SerializeField] private float LookSensitivity = 0.5f;
    private float CamX;
    private float CamY;
    private float VerticleRotation;
    private float HorozontalRotation;

    private float Fov_Max = 90;
    private float Fov_Min = 80;

    [Header("Status")]
    [SerializeField] private bool isGround_bool;

    [Header("Coroutine Related")]
    public Coroutine Mobility_coro;
    #endregion

    #region RUNTIME
    public void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
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

        if (shoot_script_ref != null)
        {
            shoot_script_ref.ShootingLogic();
        }
    }
    #endregion

    #region INPUT ACTION CALLBACKS
    public void Movement(InputAction.CallbackContext context)
    {
        Movement_Vector = context.ReadValue<Vector2>();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void PlayerLook(InputAction.CallbackContext context)
    {
        LookVector = context.ReadValue<Vector2>();
    }

    public void Shooting(InputAction.CallbackContext context)
    {
        if (shoot_script_ref != null)
        {
            shoot_script_ref.isShooting = context.ReadValueAsButton();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && Mobility_coro == null)
        {
            Mobility_coro = StartCoroutine(JumpLogic());
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        // Only dash if button is pressed and we aren't currently locked in another move action
        if (context.performed && !isDashing && Mobility_coro == null)
        {
            Dash_Dir = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            if (Dash_Dir == Vector3.zero) Dash_Dir = transform.forward; // Fallback if stationary

            Mobility_coro = StartCoroutine(Dash_Logic());
        }
    }
    #endregion

    #region LOGIC
    public void MoveLogic()
    {
        // Don't process standard movement input overrides while dashing
        if (isDashing) return;

        motion_Direction = Movement_Vector.x * transform.right + Movement_Vector.y * transform.forward;

        if (controller.isGrounded == true && Player_vert.y < 0 && isJumping == false)
        {
            Player_vert.y = DownForce;
        }
        else if (controller.isGrounded == false && isJumping == false)
        {
            Player_vert.y += Physics.gravity.y * Time.deltaTime;
        }

        Move_Collab = (motion_Direction * MoveSpeed) + new Vector3(0, Player_vert.y, 0);
        controller.Move(Move_Collab * Time.deltaTime);

        SprintLogic(isSprinting);
    }

    public void SprintLogic(bool sprinting)
    {
        if (sprinting)
        {
            MoveSpeed = SprintSpeed;
            if (Playercam != null) Playercam.fieldOfView = Fov_Max;
        }
        else
        {
            MoveSpeed = SpeedReset;
            if (Playercam != null) Playercam.fieldOfView = Fov_Min;
        }
    }

    public IEnumerator JumpLogic()
    {
        if (controller.isGrounded == true)
        {
            isJumping = true;
            Player_vert.y = JumpForce;

            // Wait until player starts falling or leaves ground
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => controller.isGrounded == true);

            isJumping = false;
            Mobility_coro = null;
        }
        else
        {
            Mobility_coro = null;
        }
    }

    public IEnumerator Dash_Logic()
    {
        isDashing = true;
        DashMotion = DashForce * Dash_Dir;
        float timer = 0f;

        if (Playercam != null) Playercam.fieldOfView = Fov_Max;

        while (timer < DashDuration)
        {
            controller.Move(DashMotion * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        if (Playercam != null) Playercam.fieldOfView = Fov_Min;
        isDashing = false;
        Mobility_coro = null;
    }
    #endregion

    #region CAMERA STUFF
    public void CameraLogic()
    {
        if (Playercam == null) return;

        CamX = LookVector.x * LookSensitivity;
        CamY = LookVector.y * LookSensitivity;

        VerticleRotation -= CamY;
        VerticleRotation = Mathf.Clamp(VerticleRotation, -90f, 90f);
        HorozontalRotation += CamX;

        Playercam.transform.localRotation = Quaternion.Euler(VerticleRotation, 0, 0);
        transform.Rotate(Vector3.up * CamX);
    }
    #endregion
}