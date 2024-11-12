using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;

    private InputAction jumpAction;
    private InputAction attackAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    private void OnEnable()
    {
        jumpAction.performed += OnJump;
        attackAction.performed += OnAttack;
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJump;
        attackAction.performed -= OnAttack;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumping");
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attacking");
    }
}
