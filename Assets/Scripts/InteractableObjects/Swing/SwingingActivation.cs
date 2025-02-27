using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingingActivation : MonoBehaviour
{
    public GameObject playerParent;
    private PlayerController playerController;
    public GameObject lumenSwing;
    public TestRotationSwing swingAction;
    private bool swinging = false;

    [SerializeField] private LumenThoughtBubbleActivation lumenThoughtBubbleActivation;

    [SerializeField] public RangeCheck rangeCheck;

    void Start()
    {
        playerController = playerParent.GetComponentInChildren<PlayerController>();
    }

    public void OnDisableSwing()
    {
        lumenThoughtBubbleActivation.enabled = false;
        this.enabled = false;
    }
    
    public void OnSwingingStart(InputAction.CallbackContext context)
    {
        Debug.Log(rangeCheck.gameObject.name);
        Debug.Log(rangeCheck.inRange);
        if (context.performed)
        {        
            if (!swinging && rangeCheck.inRange)
            {
                playerParent.SetActive(false);
                lumenSwing.SetActive(true);
                swingAction.enabled = true;
                swinging = true;
                lumenThoughtBubbleActivation.DeactivatePrompt();
                lumenThoughtBubbleActivation.enabled = false;
            }
        }
    }

    public void OnSwingingEnd(InputAction.CallbackContext context)
    {
        if (context.performed && swinging)
        {
            swinging = false;
            playerParent.SetActive(true);
            playerParent.transform.position = lumenSwing.transform.position;
            lumenSwing.SetActive(false);
            swingAction.enabled = false;
            lumenThoughtBubbleActivation.enabled = true;
        }
        
    }

    public void OnJumpOff(InputAction.CallbackContext context)
    {
        if (context.performed && swinging)
        {
            swinging = false;
            playerParent.SetActive(true);
            playerParent.transform.position = lumenSwing.transform.position;
            lumenSwing.SetActive(false);
            swingAction.enabled = false;
            lumenThoughtBubbleActivation.enabled = true;
            playerController.OnJump(context);
        }
    }
}
