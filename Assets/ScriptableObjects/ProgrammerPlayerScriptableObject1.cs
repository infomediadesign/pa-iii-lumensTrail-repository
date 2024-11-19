using UnityEngine;

[CreateAssetMenu(fileName = "ProgrammerlayerData", menuName = "ScriptableObjects/ProgrammerPlayerScriptableObject")]
public class ProgrammerPlayerScriptableObject : ScriptableObject
{
    public bool isDashing = false;

    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool jumpButtonPressed = false;
    public float groundCoyoteTimeCounter = 0;
    public float wallCoyoteTimeCounter = 0;
}
