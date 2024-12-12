using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgrammerlayerData", menuName = "ScriptableObjects/ProgrammerPlayerScriptableObject")]
public class ProgrammerPlayerScriptableObject : ScriptableObject
{
    public bool isDashing = false;

    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool jumpButtonPressed = false;
    public bool lightThrowButtonPressed = false;
    public float groundCoyoteTimeCounter = 0;
    public float wallCoyoteTimeCounter = 0;
    public float fallingVelocity = 0;
    [Unity.Collections.ReadOnly]
    public StateMachine.StateKey stateKey = StateMachine.StateKey.Grounded; 
}
