using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingingActivation : MonoBehaviour
{
    public GameObject playerParent;
    [SerializeField] private Transform playerTransform;
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
            if (!swinging && rangeCheck.inRange && playerController.itemManager.carriedItem == null && !playerController.inChase)
            {
                playerParent.SetActive(false);
                lumenSwing.SetActive(true);
                swingAction.enabled = true;
                swinging = true;
                lumenThoughtBubbleActivation.DeactivatePrompt();
                //lumenThoughtBubbleActivation.enabled = false;
            }
        }
    }

    public void OnSwingingEnd(InputAction.CallbackContext context)
    {
        if (context.performed && swinging)
        {
            swinging = false;
            playerParent.SetActive(true);
            playerTransform.position = lumenSwing.transform.position;
            lumenSwing.SetActive(false);
            swingAction.enabled = false;
            lumenThoughtBubbleActivation.ActivatePrompt();
            //lumenThoughtBubbleActivation.enabled = true;
        }
        
    }

    public void OnJumpOff(InputAction.CallbackContext context)
    {
        if (context.performed && swinging)
        {
            swinging = false;
            playerParent.SetActive(true);
            playerTransform.position = lumenSwing.transform.position;
            lumenSwing.SetActive(false);
            swingAction.enabled = false;
            //lumenThoughtBubbleActivation.enabled = true;
            lumenThoughtBubbleActivation.ActivatePrompt();
            playerController.OnJump(context);
        }
    }
}
