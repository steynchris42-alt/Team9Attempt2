using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
  
    
    //Input actions
    protected Vector2 Movement_Vector;

    //player speed settings
    protected float MoveSpeed = 5.0f;
    //Directional vectors
    Vector3 Forward = Vector3.forward;
    Vector3 Left = Vector3.left;
    Vector3 Right = Vector3.right;
    Vector3 Back = Vector3.back;
    Vector3 motion;
    public void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    public void Update()
    {
        MoveLogic();
     }
    
  public void Movement(InputAction.CallbackContext context)
    {
        Movement_Vector = context.ReadValue<Vector2>();
    }
    public void MoveLogic()
    {
        motion = Movement_Vector.x * Right + Movement_Vector.y * Forward ;
        CollisionFlags collisionFlags = controller.Move(motion * MoveSpeed * Time.deltaTime);
        Debug.Log("movementLogicCalled");
    }
}
