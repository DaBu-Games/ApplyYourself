public class RunningState : WalkingState
{
   public RunningState(PlayerInput player, PlayerValues values)
      : base(player, values)
   {
      _maxSpeed = values.MaxRunSpeed;
      _acceleration = values.RunAcceleration;
   }
}