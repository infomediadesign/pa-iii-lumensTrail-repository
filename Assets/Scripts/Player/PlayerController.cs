using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;
    private StateMachine playerStateMachine;
    private Rigidbody2D rb;
    public DesignerPlayerScriptableObject dData;
    public ProgrammerPlayerScriptableObject pData;

    private float horizontalMovement;
    private bool isMoving = false;
    private float lastTimeDashed;
    private bool isDashOnCooldown;
    public Image dashCooldownImage;

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
        lastTimeDashed = -dData.dashCooldown;
    }

    private void Update()
    {
        isGrounded();
        isTouchingWall();

        if (isDashOnCooldown) DashOnCooldown();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            if (pData.isDashing) return;
            playerStateMachine.OnMove(horizontalMovement);
        }
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pData.jumpButtonPressed = true;
            if (pData.groundCoyoteTimeCounter > 0 || pData.wallCoyoteTimeCounter > 0)
            {
                playerStateMachine.ChangeState(StateMachine.StateKey.Jumping);

                pData.groundCoyoteTimeCounter = 0;
                pData.wallCoyoteTimeCounter = 0;
            }
            
        }
        else if (context.canceled)
        {
            pData.jumpButtonPressed = false;
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
            rb.velocity = Vector2.up * rb.velocity;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < lastTimeDashed + dData.dashCooldown) return;

            lastTimeDashed = Time.time;
            playerStateMachine.ChangeState(StateMachine.StateKey.Dashing);
            isDashOnCooldown = true;
            dashCooldownImage.fillAmount = 0;
        }
    }

    private void DashOnCooldown()
    {
        float timeSinceDashed = Time.time - lastTimeDashed;
        float cooldownProgress = timeSinceDashed / dData.dashCooldown;
        dashCooldownImage.fillAmount = cooldownProgress;

        if (cooldownProgress >= 1f)
        {
            isDashOnCooldown = false;
            dashCooldownImage.fillAmount = 1f;
        }
        }

    private void isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, footBoxSize, 0, -transform.up, castDistance, groundLayer))
        {
            pData.isGrounded = true;
            pData.groundCoyoteTimeCounter = dData.coyoteTime;
        }
        else
        {
            pData.isGrounded = false;
            pData.groundCoyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void isTouchingWall()
    {
        if (Physics2D.BoxCast(transform.position, leftSideBoxSize, 0, -transform.right, castDistance, wallLayer) || Physics2D.BoxCast(transform.position, rightSideBoxSize, 0, transform.right, castDistance, wallLayer))
        {
            pData.isTouchingWall = true;
            pData.wallCoyoteTimeCounter = dData.coyoteTime;
        }
        else
        {
            pData.isTouchingWall = false;
            pData.wallCoyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, footBoxSize);
        Gizmos.DrawWireCube(transform.position + transform.right * castDistance, rightSideBoxSize);
        Gizmos.DrawWireCube(transform.position - transform.right * castDistance, leftSideBoxSize);
    }
}
