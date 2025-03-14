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
    private bool easterEggCounting = false;
    private float easterEggTimer = 0;
    private float originalDragForce;
    private float easterEggDragForce = 30f;
    private float easterEggActivationTime = 50f;

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
                easterEggCounting = true;
                easterEggDragForce = Time.time;
                originalDragForce = lumenSwing.GetComponent<TestRotationSwing>().dragForce;
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
            lumenSwing.GetComponent<TestRotationSwing>().dragForce = originalDragForce;
            easterEggCounting = false;
            easterEggTimer = 0;
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
            lumenSwing.GetComponent<TestRotationSwing>().dragForce = originalDragForce;
            easterEggCounting = false;
            easterEggTimer = 0;
        }
    }

    void Update()
    {
        if (easterEggCounting)
        {
            Debug.Log("Easteregg Timer");
            if (Time.time > easterEggTimer + easterEggActivationTime) 
            {
                Debug.Log("EaasterEgg now");
                lumenSwing.GetComponent<TestRotationSwing>().dragForce = easterEggDragForce;
                easterEggCounting = false;
                easterEggTimer = 0;
            }
        }
    }
}
