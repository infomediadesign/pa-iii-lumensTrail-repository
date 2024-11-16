using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachine sm;

    public void OnStateEnter(StateMachine stateMachine)
    {
        // this code will always be called
        sm = stateMachine;
        OnEnter();
    }

    protected virtual void OnEnter()
    {
        // this code can be overwritten in the derived classes
    }

    public void OnStateUpdate()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    public void OnStateMove()
    {
        OnMove();
    }

    protected virtual void OnMove()
    {
        float targetSpeed = sm.horizontalMovement * sm.data.moveSpeed;
        float speedDif = targetSpeed - sm.rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? sm.data.acceleration : sm.data.decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, sm.data.velPower) * Mathf.Sign(speedDif);

        sm.rb.AddForce(movement * Vector2.right);
    }

    public void OnStateExit()
    {
        OnExit();
    }

    protected virtual void OnExit() 
    {
    
    }
}
