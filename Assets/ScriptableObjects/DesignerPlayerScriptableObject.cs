using UnityEngine;

[CreateAssetMenu(fileName = "DesignerPlayerData", menuName = "ScriptableObjects/DesignerPlayerScriptableObject")]
public class DesignerPlayerScriptableObject : ScriptableObject
{
    public float moveSpeed = 10f;
    public float acceleration = 7f;
    public float decceleration = 7f;
    public float velPower = 0.9f;
    public float dashForce = 20f;
    public float frictionAmount = 0.6f;
    public float airFrictionAmount = 0.5f;
    public float dashingTime = 1f;
    public float dashCooldown = 5f;

    public float jumpForce = 9f;
    public float coyoteTime = 0.2f;
    [Range(0f, 1f)]
    public float jumpCutMultiplier = 0.5f;
    public float fallGravityMultiplier = 1.2f;
    [Range(0f, 1f)]
    public float fastFallingMultiplier = 1f;

}
