using UnityEngine;

public class IdleState : IState
{
    private PlayerInput _player;
    private PlayerValues _values;
    
    public IdleState(PlayerInput player, PlayerValues values)
    {
        _player = player;
        _values = values;
    }

    public void OnEnterState()
    {
        
    }

    public void OnExitState() { }
    public void OnUpdate() { }

    public void OnFixedUpdate()
    {
        Decelerate();
    }

    private void Decelerate()
    {
        Vector3 current = _player.RB.linearVelocity;
        Vector3 horizontal = new Vector3(current.x, 0f, current.z);
        
        Vector3 newHorizontal = Vector3.MoveTowards(
            horizontal, 
            Vector3.zero,
            _values.Deceleration * Time.fixedDeltaTime
        );

        _player.RB.linearVelocity = new Vector3(newHorizontal.x, current.y, newHorizontal.z);
    }
}