using UnityEngine;

public class JumpingState : IState
{
    private PlayerInput _player;
    private PlayerValues _values;
    
    private bool _canJump;
    
    public JumpingState(PlayerInput player, PlayerValues values)
    {
        _player = player;
        _values = values;
    }

    public void OnEnterState()
    {
        _canJump = true;
    }

    public void OnExitState() { }

    public void OnUpdate() { }

    public void OnFixedUpdate()
    {
        if (_canJump)
        {
            Jump();
            _canJump = false;
        }
    }

    private void Jump()
    {
        _player.IsJumping = true;
        
        float force = _values.JumpForce;
        
        if ( _player.RB.linearVelocity.y < 0)
        {
            force -= _player.RB.linearVelocity.y;
        }
        
        _player.RB.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}