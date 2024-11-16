using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;
    private StateMachine playerStateMachine;
    private Rigidbody2D rb;
    public PlayerScriptableObject data;

    private float horizontalMovement;
    private bool isMoving = false;
    private bool canDash = true;

    public Vector2 footBoxSize;
    public Vector2 leftSideBoxSize;
    public Vector2 rightSideBoxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public LayerMask wallLayer;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerStateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        isGrounded();
        isTouchingWall();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (data.isDashing) return;
            playerStateMachine.OnMove(horizontalMovement);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            data.jumpButtonPressed = true;
            if (data.isGrounded || data.isTouchingWall)
            {
                playerStateMachine.ChangeState(StateMachine.StateKey.Jumping);
            }
            
        }
        else if (context.canceled)
        {
            data.jumpButtonPressed = false;
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canDash) StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Start Dashing");
        canDash = false;
        playerStateMachine.ChangeState(StateMachine.StateKey.Dashing);
        yield return new WaitForSeconds(data.dashCooldown);
        canDash = true;
    }

    private void isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, footBoxSize, 0, -transform.up, castDistance, groundLayer))
        {
            data.isGrounded = true;
        }
        else
        {
            data.isGrounded = false;
        }
    }

    private void isTouchingWall()
    {
        if (Physics2D.BoxCast(transform.position, leftSideBoxSize, 0, -transform.right, castDistance, wallLayer) || Physics2D.BoxCast(transform.position, rightSideBoxSize, 0, transform.right, castDistance, wallLayer))
        {
            data.isTouchingWall = true;
        }
        else
        {
            data.isTouchingWall = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, footBoxSize);
        Gizmos.DrawWireCube(transform.position + transform.right * castDistance, rightSideBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.right * castDistance, leftSideBoxSize);
    }
}
