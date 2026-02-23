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
    public void OnEnterState() { }

    public void OnExitState() { }

    public void OnUpdate(){}

    public void OnFixedUpdate()
    {
        Climb();
    }

    private void Climb()
    {
        Vector3 gravityDir = _wallCheck.GetWallDirection.normalized;
            
        Vector3 input = _player.transform.right * _player.MoveInput.x + _player.transform.forward * _player.MoveInput.y;
        
        Vector3 surfaceMove = Vector3.ProjectOnPlane(input, gravityDir).normalized;

        Vector3 targetVelocity = surfaceMove * _values.MaxClimbSpeed;

        Vector3 current = _player.RB.linearVelocity;
        
        Vector3 surfaceVelocity = Vector3.ProjectOnPlane(current, gravityDir);

        Vector3 newSurfaceVelocity = Vector3.MoveTowards(
            surfaceVelocity,
            targetVelocity,
            _values.ClimbAcceleration * Time.fixedDeltaTime
        );
        
        Vector3 gravityVelocity = Vector3.Project(current, gravityDir);

        _player.RB.linearVelocity = newSurfaceVelocity + gravityVelocity;
    }
}