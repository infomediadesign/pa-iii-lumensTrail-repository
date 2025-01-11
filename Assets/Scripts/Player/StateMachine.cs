using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

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
        //states.AddRange(new BaseState[] { new GroundedState(this), new JumpingState(this), new AirborneState(this), new LandingState(this), new AttackingState(this), new DashState(this), new WallClingState(this), new LightThrowState(this), new LightWaveState(this), new PickupState(this), new CarryingState(this) });
        
        movementStates.AddRange(new MovementBaseState[] { new StillState(this), new MovingState(this) });
        physicsStates.AddRange(new PhysicsBaseState[] { new GroundedState(this), new JumpingState(this), new AirborneState(this), new LandingState(this) });
        actionStates.AddRange(new ActionBaseState[] { new IdleState(this), new PickupState(this), new CarryingState(this), new LightThrowState(this), new LightWaveState(this) });
    }

    private void Start()
    {
        lastMovementState = currentMovementState = movementStates[0];
        lastPhysicsState = currentPhysicsState = physicsStates[0];
        lastActionState = currentActionState = actionStates[0];
        
    }

    private void FixedUpdate()
    {
        currentMovementState?.OnUpdate();
        currentPhysicsState?.OnUpdate();
        currentActionState?.OnUpdate();

        //if (!hasLeftWallClState && !pData.isTouchingWall) hasLeftWallClState = true;
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
        if (stateKey.GetType() == typeof(MovementBaseState.StateKey))
        {
            lastMovementState = currentMovementState;
            currentMovementState = movementStates[Convert.ToInt32(stateKey)];
            lastMovementState?.OnExit();
            currentMovementState?.OnEnter();
            pData.movementStateKey = (MovementBaseState.StateKey)stateKey;
        }
        else if (stateKey.GetType() == typeof(PhysicsBaseState.StateKey))
        {
            lastPhysicsState = currentPhysicsState;
            currentPhysicsState = physicsStates[Convert.ToInt32(stateKey)];
            lastPhysicsState?.OnExit();
            currentPhysicsState?.OnEnter();
            pData.physicsStateKey = (PhysicsBaseState.StateKey)stateKey;
        }
        else if (stateKey.GetType() == typeof(ActionBaseState.StateKey))
        {
            lastActionState = currentActionState;
            currentActionState = actionStates[Convert.ToInt32(stateKey)];
            lastActionState?.OnExit();
            currentActionState?.OnEnter();
            pData.actionStateKey = (ActionBaseState.StateKey)stateKey;
        }
        else
        {
            throw new System.Exception("Invalid state key");
        }
    }

    public void ChangeToLastState(BaseState.StateType stateType)
    {
        switch (stateType)
        {
            case BaseState.StateType.Movement:
                MovementBaseState movementTransition = lastMovementState;
                lastMovementState = currentMovementState;
                currentMovementState = movementTransition;
                lastMovementState?.OnExit();
                currentMovementState?.OnEnter();
                break;
            case BaseState.StateType.Physics:
                PhysicsBaseState physicsTransition = lastPhysicsState;
                lastPhysicsState = currentPhysicsState;
                currentPhysicsState = physicsTransition;
                lastPhysicsState?.OnExit();
                currentPhysicsState?.OnEnter();
                break;
            case BaseState.StateType.Action:
                ActionBaseState actionTransition = lastActionState;
                lastActionState = currentActionState;
                currentActionState = actionTransition;
                lastActionState?.OnExit();
                currentActionState?.OnEnter();
                break;
        }
    }

    public void SetHorizontalMovement(float _horizontalMovement)
    {
        horizontalMovement = _horizontalMovement;
    }
}
