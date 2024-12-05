using UnityEngine;

[CreateAssetMenu(fileName = "DesignerPlayerData", menuName = "ScriptableObjects/DesignerPlayerScriptableObject")]
public class DesignerPlayerScriptableObject : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float acceleration = 7f;
    public float decceleration = 7f;
    public float frictionAmount = 0.6f;
    public float velPower = 0.9f;

    [Space(15)]
    [Header("Gravity, Jumping and Falling")]
    [Header("Gravity")]
    public float generalGravityMultiplier = 2f;
    public float fallGravityMultiplier = 1.2f;
    [Header("Falling")]
    [Range(0f, 1f)]
    public float fastFallingMultiplier = 1f;
    public float coyoteTime = 0.2f;
    public float airFrictionAmount = 0.5f;
    [Header("Jumping")]
    public float jumpForce = 9f;
    [Range(0f, 1f)]
    public float jumpCutMultiplier = 0.5f;

    [Space(10)]
    [Header("Dashing")]
    public float dashForce = 20f;
    public float dashingTime = 1f;
    public float dashCooldown = 5f;

    [Space(10)]
    [Header("Wall-Cling")]
    public float wallClingSlideGravityReduction = 2f;
    public float wallClingAirFreezeTime = 0.5f;

    [Space(15)]
    [Header("Light Mechanics")]
    [Header("Light Throw")]
    public float lightThrowProjectileSpeed = 5f;
    public float lightThrowProjectileMaxTravelTime = 10f;
    public float lightThrowCooldown = 3f;
    public float startChargingDelay = 0.5f;
    public float switchToLightWaveTime = 1f;
    public float lightThrowGravityMultiplier = 1f;
}
