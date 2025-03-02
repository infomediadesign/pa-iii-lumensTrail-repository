using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwingSeatRotation : MonoBehaviour
{
    public HingeJoint2D joint;
    private JointAngleLimits2D limits;

    void Start()
    {
        limits = joint.limits;
    }



    // Update is called once per frame
    void Update()
    {
        if (joint.limitState == JointLimitState2D.UpperLimit || joint.limitState == JointLimitState2D.LowerLimit)
        {
            joint.attachedRigidbody.freezeRotation = true;
        }
    }
}
