using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    /*public enum StateKey
    {
        Grounded = 0,
        Jumping = 1,
        Airborne = 2,
        Landing = 3,
        Attacking = 4,
        Dashing = 5,
        WallClinging = 6,
        LightThrow = 7,
        LightWave = 8,
        PickUp = 9,
        Carrying = 10
    }*/

    
    /*public BaseState currentState { get; private set; }
    public BaseState lastState { get; private set; }
*/
    public MovementBaseState currentMovementState { get; private set; }
    public MovementBaseState lastMovementState { get; private set; }

    public PhysicsBaseState currentPhysicsState { get; private set; }
    public PhysicsBaseState lastPhysicsState { get; private set; }

    public ActionBaseState currentActionState { get; private set; }
    public ActionBaseState lastActionState { get; private set; }




    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public SpriteRenderer sr;
    [HideInInspector]
    public TrailRenderer tr;
    [HideInInspector]
    public PlayerLightThrowManager ltm;
    [HideInInspector]
    public ItemManager im;
    

    public DesignerPlayerScriptableObject dData;
    public ProgrammerPlayerScriptableObject pData;

    /*public List<BaseState> states { get; private set; } = new List<BaseState>();*/

    public List<MovementBaseState> movementStates { get; private set; } = new List<MovementBaseState>();
    public List<PhysicsBaseState> physicsStates { get; private set; } = new List<PhysicsBaseState>();
    public List<ActionBaseState> actionStates { get; private set; } = new List<ActionBaseState>();


    //public GroundedState groundedState = new GroundedState();
    //public JumpingState jumpingState = new JumpingState();
    //public AirborneState airborneState = new AirborneState();
    //public LandingState landingState = new LandingState();
    //public AttackingState attackingState = new AttackingState();
    //public DashState dashState = new DashState();
    //public WallClingState wallClState = new WallClingState();

    public float horizontalMovement { get; private set; } = 0;
    public bool hasLeftWallClState = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        ltm = GetComponent<PlayerLightThrowManager>();
        states.AddRange(new BaseState[] { new GroundedState(this), new JumpingState(this), new AirborneState(this), new LandingState(this), new AttackingState(this), new DashState(this), new WallClingState(this), new LightThrowState(this), new LightWaveState(this), new PickupState(this), new CarryingState(this) });
    }

    private void Start()
    {
        lastState = states[0];
        ChangeState(StateKey.Grounded);
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }

        if (!hasLeftWallClState && !pData.isTouchingWall) hasLeftWallClState = true;
    }

    public void SwitchToState(Enum stateKey)
    {
        if (stateKey.GetType() == typeof(MovementBaseState.StateKey))
        {
            movementStates[Convert.ToInt32(stateKey)].SwitchTo();
        } 
        else if (stateKey.GetType() == typeof(PhysicsBaseState.StateKey))
        {
            physicsStates[Convert.ToInt32(stateKey)].SwitchTo();
        }
        else if (stateKey.GetType() == typeof(ActionBaseState.StateKey))
        {
            actionStates[Convert.ToInt32(stateKey)].SwitchTo();
        }
        else
        {
            throw new System.Exception("Invalid state key");
        }
    }

    public void ChangeState(Enum stateKey)
    {
        lastState = currentState;
        currentState = states[Convert.ToInt32(stateKey)];

        lastState?.OnExit();
        currentState.OnEnter();
        if (stateKey.GetType() == typeof(MovementBaseState.StateKey))
        {
            pData.movementStateKey = (MovementBaseState.StateKey)stateKey;
        }
        else if (stateKey.GetType() == typeof(PhysicsBaseState.StateKey))
        {
            pData.physicsStateKey = (PhysicsBaseState.StateKey)stateKey;
        }
        else if (stateKey.GetType() == typeof(ActionBaseState.StateKey))
        {
            pData.actionStateKey = (ActionBaseState.StateKey)stateKey;
        }
    }

    public void ChangeToLastState()
    {
        currentState.OnExit();
        currentState = lastState;
    }

    public void OnMove(float _horizontalMovement)
    {
        horizontalMovement = _horizontalMovement;
        currentState.OnMove();
    }
}
