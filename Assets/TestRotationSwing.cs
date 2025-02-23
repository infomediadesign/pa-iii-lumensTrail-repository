using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationSwing : MonoBehaviour
{
    
    public Joint2D joint;
    public Rigidbody2D swingbody;

    public Rigidbody2D jointBody;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("d"))
        {
            swingbody.AddRelativeForce(Vector2.right * 0.5f, ForceMode2D.Force);
            swingbody.rotation = jointBody.rotation;
        }
        if (Input.GetKey("a"))
        {
            swingbody.AddRelativeForce(Vector2.left * 0.5f, ForceMode2D.Force);
            swingbody.rotation = jointBody.rotation;
        }
        
    }
}
