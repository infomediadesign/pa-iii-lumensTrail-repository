using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PickupState : ActionBaseState
{
    private GameObject carriedObject;
    private float arcHeight = 1f;
    private float duration = 1f;
    private float heightGap = 0.5f;
    private Vector2 startPoint;
    private Vector2 endPoint;

    private float elapsedTime = 0;
    
    public PickupState(StateMachine sm): base(sm) 
    {
        ownState = ActionBaseState.StateKey.PickUp;
    }

    public override void SwitchTo()
    {
        if ((PhysicsBaseState.StateKey)sm.currentPhysicsState.ownState != PhysicsBaseState.StateKey.Grounded) return;  
        if ((ActionBaseState.StateKey)sm.currentActionState.ownState != ActionBaseState.StateKey.Idle) return;
        base.SwitchTo();
    }

    public override void OnEnter()
    {
        sm.animator.SetBool("pickup", true);
        arcHeight = sm.dData.pickupArcHeight;
        duration = sm.dData.pickupArcSpeed;
        heightGap = sm.dData.aboveHeadCarryGap;

        carriedObject = sm.im.carriedItem;
        float endHeight = sm.transform.position.y + sm.sr.bounds.size.y / 6 + carriedObject.GetComponent<SpriteRenderer>().bounds.size.y / 2 + heightGap;
        endPoint = new Vector2(sm.transform.position.x, endHeight);
        startPoint = carriedObject.transform.position;
        elapsedTime = 0;

        MovementBaseState.movementEnabled = false;
    }

    public override void OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        Vector2 currentPosition = CalculateArcPosition(startPoint, endPoint, arcHeight, t);
        carriedObject.transform.position = currentPosition;

        if (t >= 1)
        {
            carriedObject.transform.position = endPoint;
            sm.SwitchToState(ActionBaseState.StateKey.Carrying);
        }
    }

    public override void OnExit()
    {
        MovementBaseState.movementEnabled = true;
        sm.animator.SetBool("pickup", false);
    }

    private Vector2 CalculateArcPosition(Vector2 start, Vector2 end, float arcHeight, float t)
    {
        // Linear interpolation between start and end
        Vector2 linearPoint = Vector2.Lerp(start, end, t);

        // Add the parabolic arc offset to the Y component
        float arc = 4 * arcHeight * (t - t * t); // Parabola: h(t) = 4h * (t - t^2)
        linearPoint.y += arc;

        return linearPoint;
    }



}
