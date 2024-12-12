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

    

    public virtual void OnEnter()
    {
        // this code can be overwritten in the derived classes
    }


    public virtual void OnUpdate()
    {
        // this code can be overwritten in the derived classes
    }

    public virtual void OnMove()
    {
        float targetSpeed = sm.horizontalMovement * sm.dData.moveSpeed;
        // for acceleration and decceleration, but currently not used
        //float speedDif = targetSpeed - sm.rb.velocity.x;
        //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? sm.data.acceleration : sm.data.decceleration;
        //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, sm.data.velPower) * Mathf.Sign(speedDif);

        sm.rb.velocity = (Vector2.right * targetSpeed + Vector2.up * sm.rb.velocity);
    }

    
    public virtual void OnExit()
    {
        // this code can be overwritten in the derived classes
    }


}
