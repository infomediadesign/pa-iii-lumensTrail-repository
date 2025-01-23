using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgrammerlayerData", menuName = "ScriptableObjects/ProgrammerPlayerScriptableObject")]
public class ProgrammerPlayerScriptableObject : ScriptableObject
{
    [Unity.Collections.ReadOnly]
    public bool isGrounded = false;
    [Unity.Collections.ReadOnly]
    public bool jumpButtonPressed = false;
    [Unity.Collections.ReadOnly]
    public bool lightThrowButtonPressed = false;
    [Unity.Collections.ReadOnly]
    public float groundCoyoteTimeCounter = 0;
    [Unity.Collections.ReadOnly]
    public float fallingVelocity = 0;
   // [Unity.Collections.ReadOnly]
    //public StateMachine.StateKey stateKey = StateMachine.StateKey.Grounded;
    
    [Unity.Collections.ReadOnly]
    public MovementBaseState.StateKey movementStateKey = MovementBaseState.StateKey.Still;
    [Unity.Collections.ReadOnly]
    public PhysicsBaseState.StateKey physicsStateKey = PhysicsBaseState.StateKey.Grounded;
    [Unity.Collections.ReadOnly]
    public ActionBaseState.StateKey actionStateKey = ActionBaseState.StateKey.Idle;

}
