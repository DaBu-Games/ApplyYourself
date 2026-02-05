using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GroundCheck _groundCheck;
    public Rigidbody RB {get; private set;}
    public Vector2 MoveInput {get; private set;}
    public bool IsHoldingJump {get; private set;}
    public bool IsJumping;
    
    private float lastPressedJumpTime;

    public PlayerInput(bool isJumping)
    {
        IsJumping = isJumping;
    }

    public bool IsHoldingRun {get; private set;}
    
    public bool IsGrounded => _groundCheck.IsGrounded;

    private void Start()
    { 
        RB = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if ( context.started )
        {
            IsHoldingJump = true;
            lastPressedJumpTime = Time.time;
        }
        else if (context.canceled)
        {
            IsHoldingJump = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        IsHoldingRun = context.performed;
    }
    
    public bool CanCyoteJump(float leaveGroundBuffer)
    {
        return Time.time - _groundCheck.LastOnGroundTime <= leaveGroundBuffer;
    }

    public bool IsJumpBufferd(float bufferTime)
    {
        return Time.time - lastPressedJumpTime <= bufferTime && Time.time > bufferTime;
    }
}
