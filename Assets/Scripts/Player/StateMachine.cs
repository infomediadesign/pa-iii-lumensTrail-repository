using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState currentState;
    [HideInInspector]
    public CharacterController characterController;

    public GroundedState groundedState = new GroundedState();
    public JumpingState jumpingState = new JumpingState();
    public AirborneState airborneState = new AirborneState();
    public LandingState landingState = new LandingState();
    public AttackingState attackingState = new AttackingState();

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        ChangeState(groundedState);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = newState;
        currentState.OnStateEnter(this);
    }

    public void OnMove(float horizontalMovement)
    {
        currentState.OnStateMove(horizontalMovement);
    }
}
