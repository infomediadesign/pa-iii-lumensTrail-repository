using System.Collections;
using System.Runtime.CompilerServices;
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
    public ItemManager itemManager;

    private float horizontalMovement;
    private bool isFacingRight = true;
    private bool isMoving = false;
    private bool lightThrowCharge = false;
    private float lastTimeLightThrown;
    private float lastTimeLigthImpulse;
    private float pickupRadius;
    private CollectableReceiver receiver;
    private Animator animator;
    public Vector2 footBoxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    [HideInInspector] public bool inChase = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerStateMachine = GetComponent<StateMachine>();
        lastTimeLightThrown = -dData.lightThrowCooldown;
        lastTimeLigthImpulse = -dData.impulseCooldown;
        rb.gravityScale = dData.generalGravityMultiplier;
        playerStateMachine.im = itemManager;
        receiver = FindObjectOfType<CollectableReceiver>();
        animator = GetComponent<Animator>();

        //to be designer stuff
        pickupRadius = dData.pickupRange;
    }

    private void Update()
    {
        IsGrounded();

        if (lightThrowCharge)
        {
            if (Time.time < lastTimeLightThrown + dData.switchToLightWaveTime && !pData.lightThrowButtonPressed)
            {
                playerStateMachine.SwitchToState(ActionBaseState.StateKey.LightThrow);
                lightThrowCharge = false;
            }
            else if (Time.time > lastTimeLightThrown + dData.switchToLightWaveTime) 
            {
                if (pData.isGrounded) 
                {
                    playerStateMachine.SwitchToState(ActionBaseState.StateKey.LightWave);
                    lightThrowCharge = false;
                }
                else 
                {
                    lightThrowCharge = false;
                }
                
            }
        }

        /*
         * @info: When configuring jumping, uncomment line below otherwise please turn off to not mess with other code
         */
        //rb.gravityScale = dData.generalGravityMultiplier;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            playerStateMachine.SetHorizontalMovement(horizontalMovement);
        }
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && !PauseMenu.isPaused)
        {
            pData.jumpButtonPressed = true;
            if (pData.groundCoyoteTimeCounter > 0)
            {
                playerStateMachine.SwitchToState(PhysicsBaseState.StateKey.Jumping);

                pData.groundCoyoteTimeCounter = 0;
            }
            
        }
        else if (context.canceled)
        {
            pData.jumpButtonPressed = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && !PauseMenu.isPaused)
        {
            isMoving = true;
            horizontalMovement = context.ReadValue<Vector2>().x;
            // To flip the player when changing direction
            if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0)) FlipPlayerCharacter(); 
            playerStateMachine.SwitchToState(MovementBaseState.StateKey.Moving);
        }
        else if (context.canceled)
        {
            isMoving = false;
            playerStateMachine.SwitchToState(MovementBaseState.StateKey.Still);
        }
    }

    public void OnLightThrow(InputAction.CallbackContext context)
    {
        if (context.performed && !PauseMenu.isPaused)
        {
            pData.lightThrowButtonPressed = true;
            if (Time.time < lastTimeLightThrown + dData.lightThrowCooldown) return;
            lastTimeLightThrown = Time.time;
            lightThrowCharge = true;
        }
        else if (context.canceled && !PauseMenu.isPaused) 
        {
            pData.lightThrowButtonPressed = false;
        }
    }

    public void OnPickupItem(InputAction.CallbackContext context)
    {
        if (receiver == null) return;
        if (context.performed && !PauseMenu.isPaused)
        {
            if ((ActionBaseState.StateKey)playerStateMachine.currentActionState.ownState == ActionBaseState.StateKey.Carrying && pData.inDropRange == true)
            {
                receiver.DeliverItem(itemManager.carriedItem);
                receiver.GetComponentInChildren<ThoughtBubble>().SetItemDeliveredFadeTrue();
                playerStateMachine.SwitchToState(ActionBaseState.StateKey.Idle);
            }
            else
            {
                bool canPickup = false;
                GameObject pickupItem = itemManager.GetNearestPickupItem(transform, pickupRadius, isFacingRight, ref canPickup);
                if (canPickup && itemManager.carriedItem == null)
                {
                    itemManager.carriedItem = pickupItem;
                    playerStateMachine.SwitchToState(ActionBaseState.StateKey.PickUp);
                }
            }
        }
    }

    public void PickUpNow() 
    {
        pData.pickupAnimationGo = true;
    }

    public void OnLightImpulse(InputAction.CallbackContext context)
    {
        if (context.performed && !PauseMenu.isPaused)
        {
            if (Time.time < lastTimeLigthImpulse + dData.impulseCooldown) return;
            lastTimeLigthImpulse = Time.time;
            animator.SetBool("lightImpulse", true);
        }
    }

    private void IsGrounded()
    {
        if (Physics2D.OverlapBox(transform.position - transform.up * castDistance, footBoxSize, 0, groundLayer))
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
    

    private void FlipPlayerCharacter()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public bool GetIsFacingRight() 
    {
        return this.isFacingRight;
    }

    public void CutsceneWalking()
    {
        this.horizontalMovement = 1;
        this.isFacingRight = true;
        playerStateMachine.SwitchToState(MovementBaseState.StateKey.Moving);
    }

    public void CutsceneStopWalking()
    {
        this.horizontalMovement = 0;
        playerStateMachine.SwitchToState(MovementBaseState.StateKey.Still);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, footBoxSize);
    }
}
