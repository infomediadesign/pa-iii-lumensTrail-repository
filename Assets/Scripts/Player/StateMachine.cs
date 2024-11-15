using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum StateKey
    {
        Grounded,
        Jumping,
        Airborne,
        Landing,
        Attacking
    }

    private BaseState currentState;
    private BaseState lastState;
    [HideInInspector]
    public Rigidbody2D rb;

    public PlayerScriptableObject data;

    public GroundedState groundedState = new GroundedState();
    public JumpingState jumpingState = new JumpingState();
    public AirborneState airborneState = new AirborneState();
    public LandingState landingState = new LandingState();
    public AttackingState attackingState = new AttackingState();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastState = groundedState;
        ChangeState(StateKey.Grounded);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }

    public void ChangeState(StateKey stateKey)
    {
        switch(stateKey)
        {
            case StateKey.Grounded:
                lastState = currentState;
                currentState = groundedState;
                break;
            case StateKey.Jumping:
                if (currentState == groundedState || currentState == attackingState || currentState == airborneState)
                {
                    lastState = currentState;
                    currentState = jumpingState;
                }
                else return;
                break;
            case StateKey.Airborne:
                if (currentState == groundedState || currentState == jumpingState || currentState == attackingState)
                {
                    lastState = currentState;
                    currentState = airborneState;
                }
                else return;
                break;
            case StateKey.Landing:
                lastState = currentState;
                currentState = landingState;
                break;
            case StateKey.Attacking:
                lastState = currentState;
                currentState = attackingState;
                break;
        }
        lastState?.OnStateExit();
        currentState.OnStateEnter(this);
    }

    public void OnMove(float horizontalMovement)
    {
        currentState.OnStateMove(horizontalMovement);
    }
}
