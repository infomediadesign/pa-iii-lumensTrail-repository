using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachine sm;
    public StateMachine.StateKey stateKey { get; protected set; }

    public BaseState(StateMachine p_sm)
    {
        this.sm = p_sm;
    }

    public virtual void SwitchTo()
    {
        sm.ChangeState(stateKey);
    }

    public void OnStateEnter()
    {
        // this code will always be called
        
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
        float targetSpeed = sm.horizontalMovement * sm.dData.moveSpeed;
        // for acceleration and decceleration, but currently not used
        //float speedDif = targetSpeed - sm.rb.velocity.x;
        //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? sm.data.acceleration : sm.data.decceleration;
        //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, sm.data.velPower) * Mathf.Sign(speedDif);

        sm.rb.velocity = (Vector2.right * targetSpeed + Vector2.up * sm.rb.velocity);
    }

    public void OnStateExit()
    {
        OnExit();
    }

    protected virtual void OnExit() 
    {
    
    }
}
