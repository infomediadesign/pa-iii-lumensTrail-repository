using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;
    private StateMachine playerStateMachine;
    private CharacterController characterController;

    public bool isGrounded = false;
    private bool isTouchingWall = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        playerStateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        isGrounded = CheckGround();
        isTouchingWall = CheckWall();
    }

    private bool CheckGround()
    {
        // Raycast to check for collision underneath the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f);
        if (hit.collider.CompareTag("Ground")) Debug.Log("Hit");
        // First: check if Raycast hit an object, second: check if object has Tag "Ground"
        return hit.collider != null && hit.collider.CompareTag("Ground");
    }

    private bool CheckWall()
    {
        // Raycast to check for collision left and right of the player
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.3f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.3f);
        // For both sides, check if Raycast hit something and check if object has tag "Wall"
        return (hitLeft.collider != null && hitLeft.collider.CompareTag("Wall")) || (hitRight.collider != null && hitRight.collider.CompareTag("Wall"));
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump")
        }
        
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Attack");
        }
    }
}
