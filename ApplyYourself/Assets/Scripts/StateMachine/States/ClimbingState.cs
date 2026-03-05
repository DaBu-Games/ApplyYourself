using UnityEngine;

public class ClimbingState : IState
{
    private PlayerInput _player;
    private PlayerValues _values;
    private WallCheck _wallCheck;
    
    public ClimbingState(PlayerInput player, PlayerValues values, WallCheck wallCheck)
    {
        _player = player;
        _values = values;
        _wallCheck = wallCheck;
    }

    public void OnEnterState()
    {
        _player.RB.linearVelocity = Vector3.zero;
    }

    public void OnExitState() { }

    public void OnUpdate(){}

    public void OnFixedUpdate()
    {
        Climb();
    }

    private void Climb()
    {
        Vector3 wallNormal = _wallCheck.GetWallDirection;
        
        Vector3 wallRight = Vector3.Cross(wallNormal, Vector3.up).normalized;
        
        Vector3 input = wallRight * _player.MoveInput.x + Vector3.up * _player.MoveInput.y;
        
        if (input.magnitude > 1f)
            input.Normalize();
        
        Vector3 targetVelocity = input * _values.MaxClimbSpeed;
        _player.RB.linearVelocity = Vector3.MoveTowards(
            _player.RB.linearVelocity,
            targetVelocity,
            _values.ClimbAcceleration * Time.fixedDeltaTime
        );
        
        Vector3 pullForce = wallNormal * -_values.ClimbStickForce;
        _player.RB.AddForce(pullForce, ForceMode.Force);
    }
}