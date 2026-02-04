using UnityEngine;

public class WalkingState : IState
{
    private PlayerInput _player;
    protected float _maxSpeed;
    protected float _acceleration;
    
    public WalkingState(PlayerInput player, PlayerValues values)
    {
        _player = player;
        _maxSpeed = values.MaxWalkSpeed;
        _acceleration = values.WalkAcceleration;
    }
    public void OnEnterState() { }

    public void OnExitState() { }

    public void OnUpdate(){}

    public void OnFixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 input = _player.transform.right * _player.MoveInput.x + _player.transform.forward * _player.MoveInput.y;

        Vector3 targetVelocity = input * _maxSpeed;

        Vector3 current = _player.RB.linearVelocity;
        Vector3 horizontal = new Vector3(current.x, 0f, current.z);

        Vector3 newHorizontal = Vector3.MoveTowards(
            horizontal,
            targetVelocity,
            _acceleration * Time.fixedDeltaTime
        );

        _player.RB.linearVelocity = new Vector3(
            newHorizontal.x,
            current.y,
            newHorizontal.z
        );
    }
}