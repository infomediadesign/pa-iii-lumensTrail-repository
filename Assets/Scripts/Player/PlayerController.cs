using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;
    private StateMachine playerStateMachine;
    private CharacterController characterController;

    private float horizontalMovement;
    public float moveSpeed = 5f;
    public float gravity = 0.1f;
    private bool isMoving = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        playerStateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if (isMoving)
        {
            playerStateMachine.OnMove(horizontalMovement * moveSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(Vector3.down * gravity);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump");
        }
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Attack");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMoving = true;
            horizontalMovement = context.ReadValue<Vector2>().x;
            
        }
        else if (context.canceled)
        {
            isMoving = false;
        }
    }
}
