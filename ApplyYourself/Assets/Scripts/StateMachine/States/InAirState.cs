using UnityEngine;

public class InAirState : IState
{
    private PlayerInput _player;
    private PlayerValues _values;
    
    public InAirState(PlayerInput player, PlayerValues values)
    {
        _player = player;
        _values = values;
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
        Vector3 input = _player.transform.right * _player.MoveInput.x + _player.transform.forward * _player.MoveInput.y;
        
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
}
