using UnityEngine;

public class WalkingState : IState
{
    private PlayerInput _player;
    private PlayerValues _values;
    private Transform _cameraTransform;
    protected float _maxSpeed;
    protected float _acceleration;
    
    public WalkingState(PlayerInput player, PlayerValues values, Transform cameraTransform)
    {
        _player = player;
        _values = values;
        _maxSpeed = _values.MaxWalkSpeed;
        _acceleration = _values.WalkAcceleration;
        _cameraTransform = cameraTransform;
    }
    public void OnEnterState() { }

    public void OnExitState() { }

    public void OnUpdate(){}

    public void OnFixedUpdate()
    {
        Move();
        RotateTowardsMovement();
    }

    private void Move()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 input = cameraRight * _player.MoveInput.x + cameraForward * _player.MoveInput.y;

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