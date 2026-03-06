using UnityEngine;

public class RunningState : WalkingState
{
   public RunningState(PlayerInput player, PlayerValues values, Transform cameraTransform)
      : base(player, values, cameraTransform)
   {
      _maxSpeed = values.MaxRunSpeed;
      _acceleration = values.RunAcceleration;
   }
}