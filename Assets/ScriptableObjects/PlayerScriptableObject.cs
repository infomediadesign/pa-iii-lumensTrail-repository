using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerScriptableObject")]
public class PlayerScriptableObject : ScriptableObject
{
    public float moveSpeed = 10f;
    public float acceleration = 7f;
    public float decceleration = 7f;
    public float velPower = 0.9f;
    public float frictionAmount = 0.5f;

    public bool jumpButtonPressed = false;
    public bool isGrounded = false;
    public float jumpForce = 9f;
    [Range(0f, 1f)]
    public float jumpCutMultiplier = 0.5f;
    public float fallGravityMultiplier = 1.2f;

    public bool isTouchingWall = false;
}
