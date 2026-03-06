using UnityEngine;

public class InAirState : IState
{
    private PlayerInput _player;
    private PlayerValues _values;
    private Transform _cameraTransform;
    
    public InAirState(PlayerInput player, PlayerValues values, Transform cameraTransform)
    {
        _player = player;
        _values = values;
        _cameraTransform = cameraTransform;
    }

    public void OnEnterState() { }

    public void OnExitState()
    {
        if(_player.IsJumping)
            _player.IsJumping = false;
    }
    public void OnUpdate() { }

    public void OnFixedUpdate()
    {
        ApplyGravity();
        AirControl();
        RotateTowardsMovement();
    }

    private void ApplyGravity()
    {
        float gravity = (_player.RB.linearVelocity.y < 0 || (_player.IsJumping && !_player.IsHoldingJump))
            ? _values.FallingGravity
            : _values.Gravity;

        _player.RB.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }

    private void AirControl()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 input = cameraRight * _player.MoveInput.x + cameraForward * _player.MoveInput.y;
        
        float maxSpeed = _player.IsHoldingRun ? _values.MaxRunSpeed : _values.MaxWalkSpeed;
        
        Vector3 targetVelocity = input * maxSpeed;

        Vector3 current = _player.RB.linearVelocity;
        Vector3 horizontal = new Vector3(current.x, 0f, current.z);
        
        float acceleration = _player.IsHoldingRun ? _values.RunAcceleration : _values.WalkAcceleration;

        Vector3 newHorizontal = Vector3.MoveTowards(
            horizontal, 
            targetVelocity, 
            acceleration * Time.fixedDeltaTime
        );

        _player.RB.linearVelocity = new Vector3(
            newHorizontal.x, 
            current.y, 
            newHorizontal.z
        );
    }
    
    private void RotateTowardsMovement()
    {
        Vector3 movementDirection = new Vector3(
            _player.RB.linearVelocity.x, 
            0f, 
            _player.RB.linearVelocity.z
        );
        
        if (movementDirection.magnitude > 0.1f && _player.MoveInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            
            _player.transform.rotation = Quaternion.Slerp(
                _player.transform.rotation,
                targetRotation,
                _values.RotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}
