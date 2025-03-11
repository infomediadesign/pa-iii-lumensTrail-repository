using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestRotationSwing : MonoBehaviour
{
    
    public Joint2D joint;
    public Rigidbody2D swingbody;

    public Rigidbody2D jointBody;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool triggeredLeft = false;
    private bool triggeredRight = false;
    private bool swingingLeft = true;
    private float leverValue = 0;
    public float growthRate = 1f;
    public float decayRate = 0.1f;

    public float swingForce = 1.5f;
    public float dragForce = 0.3f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("d"))
        {
            if (!triggeredRight)
            {
                triggeredRight = true;
                swingbody.AddRelativeForce(Vector2.right * swingForce, ForceMode2D.Impulse);
                triggeredLeft = false;
            }
            swingbody.AddRelativeForce(Vector2.right * dragForce, ForceMode2D.Force);
            swingbody.rotation = jointBody.rotation;
            
        }
        else if (Input.GetKey("a"))
        {
            if (!triggeredLeft)
            {
                triggeredLeft = true;
                swingbody.AddRelativeForce(Vector2.left * swingForce, ForceMode2D.Impulse);
                triggeredRight = false;
            }
            swingbody.AddRelativeForce(Vector2.left * dragForce, ForceMode2D.Force);
            swingbody.rotation = jointBody.rotation;
            
        }
        
        if (swingbody.rotation < 0)
        {
            if (!swingingLeft)
            {
                swingingLeft = true;
                triggeredRight = false;
            }
        }
        if (swingbody.rotation > 0)
        {
            if (swingingLeft)
            {
                swingingLeft = false;
                triggeredLeft = false;
            }
        }
        if (Input.GetKey("d"))
        {
            leverValue = Mathf.MoveTowards(leverValue, 1f, growthRate * Time.fixedDeltaTime);
            Debug.Log("UP");
        }
        else if (Input.GetKey("a"))
        {
            leverValue = Mathf.MoveTowards(leverValue, -1f, growthRate * Time.fixedDeltaTime);
            Debug.Log("DOWN");
        }
        else 
        {
            leverValue = Mathf.MoveTowards(leverValue, 0f, decayRate * Time.fixedDeltaTime);
            Debug.Log("NEUTRAL");
        }
        animator.SetFloat("leverValue", leverValue);


        Debug.Log(leverValue);
    }
}
