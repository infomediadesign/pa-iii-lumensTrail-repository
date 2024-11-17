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
        Attacking,
        Dashing,
        WallClinging
    }

    private BaseState currentState;
    public BaseState lastState { get; private set; }
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public TrailRenderer tr;

    public PlayerScriptableObject data;

    public GroundedState groundedState = new GroundedState();
    public JumpingState jumpingState = new JumpingState();
    public AirborneState airborneState = new AirborneState();
    public LandingState landingState = new LandingState();
    public AttackingState attackingState = new AttackingState();
    public DashState dashState = new DashState();
    public WallClingState wallClState = new WallClingState();

    public float horizontalMovement { get; private set; } = 0;
    private bool hasLeftWallClState = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        lastState = groundedState;
        ChangeState(StateKey.Grounded);
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }

        if (!hasLeftWallClState && !data.isTouchingWall) hasLeftWallClState = true;
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
                if (currentState == airborneState) return;
                lastState = currentState;
                currentState = jumpingState; 
                break;
            case StateKey.Airborne:
                if (currentState != landingState)
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
            case StateKey.Dashing:
                lastState = currentState;
                currentState = dashState;
                break;
            case StateKey.WallClinging:
                if (!hasLeftWallClState) return;
                lastState = currentState;
                currentState = wallClState;
                hasLeftWallClState = false;
                break;
        }
        lastState?.OnStateExit();
        currentState.OnStateEnter(this);
    }

    public void ChangeToLastState()
    {
        currentState.OnStateExit();
        currentState = lastState;
    }

    public void OnMove(float _horizontalMovement)
    {
        horizontalMovement = _horizontalMovement;
        currentState.OnStateMove();
    }
}
