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
    private float lastTimeLightThrown;
    private float lastTimeLigthImpulse;
    private float pickupRadius;
    private CollectableReceiver receiver;

    public Vector2 footBoxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

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

        //to be designer stuff
        pickupRadius = dData.pickupRange;
    }

    private void Update()
    {
        IsGrounded();

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
        if (context.performed)
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
        if (context.performed)
        {
            pData.lightThrowButtonPressed = true;
            if (Time.time < lastTimeLightThrown + dData.lightThrowCooldown) return;
            lastTimeLightThrown = Time.time;
            playerStateMachine.SwitchToState(ActionBaseState.StateKey.LightThrow);
        }
        else if (context.canceled) 
        {
            pData.lightThrowButtonPressed = false;
        }
    }

    public void OnPickupItem(InputAction.CallbackContext context)
    {
        if (receiver == null) return;
        if (context.performed)
        {
            if ((ActionBaseState.StateKey)playerStateMachine.currentActionState.ownState == ActionBaseState.StateKey.Carrying && pData.inDropRange == true)
            {
                itemManager.carriedItem.transform.SetParent(GameObject.Find("Items").transform, true);
                receiver.DeliverItem(itemManager.carriedItem);
                playerStateMachine.SwitchToState(ActionBaseState.StateKey.Idle);
            }
            else
            {
                bool canPickup = true;
                GameObject pickupItem = itemManager.GetNearestPickupItem(transform, pickupRadius, isFacingRight, ref canPickup);
                if (canPickup)
                {
                    itemManager.carriedItem = pickupItem;
                    pickupItem.transform.SetParent(this.transform, true);
                    LayerMask mask = itemManager.carriedItem.GetComponent<Collider2D>().excludeLayers;
                    int layerToAdd = LayerMask.GetMask("Platform");
                    itemManager.carriedItem.GetComponent<Collider2D>().excludeLayers |= layerToAdd;
                    itemManager.carriedItem.gameObject.transform.GetChild(0).GetComponent<LumenThoughtBubbleActivation>().DeactivatePrompt();
                    playerStateMachine.SwitchToState(ActionBaseState.StateKey.PickUp);
                }
            }
        }
    }

    public void OnLightImpulse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < lastTimeLigthImpulse + dData.impulseCooldown) return;
            lastTimeLigthImpulse = Time.time;
            LightImpuls lI = GetComponentInChildren<LightImpuls>();
            if (lI != null) lI.LightImpulse();
            else Debug.Log("LightImpulse Component is null");
        }
    }

    private void IsGrounded()
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
    

    private void FlipPlayerCharacter()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, footBoxSize);
    }
}
