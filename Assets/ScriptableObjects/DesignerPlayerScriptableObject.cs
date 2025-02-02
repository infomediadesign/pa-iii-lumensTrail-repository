using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "DesignerPlayerData", menuName = "ScriptableObjects/DesignerPlayerScriptableObject")]
public class DesignerPlayerScriptableObject : ScriptableObject
{
    //[Header("Hover over the variable name or value field to see tooltips")]

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
    public float lightThrowCooldown = 3f;
    public float startChargingDelay = 0.5f;
    public float switchToLightWaveTime = 1f;
    public float lightThrowGravityMultiplier = 1f;

    [Space(10)]
    [Header("Light Wave")]
    public float lightWaveSpeed = 6f;
    public float lightWaveMaxTravelDistance = 6f;
    public float lightWaveStartingSizeMultiplier = 0.4f;
    public float lightWaveMaxSizeAtDistance = 2f;
    public float lightWaveMovementDisabledTime = 2f;
    public Vector2 lightWaveColliderSize = Vector2.zero;

    [Space(10)]
    [Header("Light Impulse")]
    public float impulseCooldown = 10f;
    public float maxImpulseRadius = 15f;
    public float impulseSpeed = 3f;
    public float highlightTime = 6f;
    public float increasedLightRadiusTime = 5f;

    [Space(15)]
    [Header("Interactable Objects")]
    [Header("Propeller Flower")]
    [Tooltip("The time the flower is actively pushing the player upwards in seconds")]
    public float propellerFlowerActiveTime = 8f;
    [Tooltip("The strength of the wind that pushes the player upwards")]
    public float maxWindStrength = 30f;

    [Space(10)]
    [Header("Items")]
    public float pickupRange = 2f;
    public float pickupArcHeight = 1f;
    public float pickupArcSpeed = 1f;
    public float aboveHeadCarryGap = 0.5f;
    public float speedModWhileCarrying = 0.5f;

    [Space(10)]
    [Header("Vines")]
    public float stayActiveTime = 10f;

    [Space(10)]
    [Header("Statue")]
    public float statueVelocityMultiplier = 2f;
}
