using Godot;
using System;

public class PlayerSlideState : State
{

    public PlayerSlideState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if ((Mathf.Abs(player.Velocity.x) < Mathf.Abs(player.WalkSpeed) || !Input.IsActionPressed("ui_down")) && player.SlideTimer.IsStopped())
        {
            if (direction != 0 && player.IsOnFloor() && !player.CeilingRaycast.IsColliding())
            {
                _stateMachine.Change("walk");
                player.WasOnGround = true;
            }
            else if (player.IsOnFloor() && !player.CeilingRaycast.IsColliding())
            {
                _stateMachine.Change("idle");
                player.WasOnGround = true;
            }
            else if (!player.IsOnFloor() && !player.CeilingRaycast.IsColliding())
            {
                _stateMachine.Change("fall");
                player.WasOnGround = false;
            }
            else
            {
                _stateMachine.Change("crouch");
                player.WasOnGround = true;
            }
        }
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, 0, 0.02f);
    }

    public override void HandleSignal(SignalType signal)
    {

    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.ExtraJumpsLeft = player.MaxExtraJumps;
        player.Velocity.x = player.SlideSpeed * player.GetPlayerDirection();
        player.AnimatedSprite.Play("slide");
        player.ToggleCrouchCollisionShape();
        player.SlideTimer.Start();
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;
        player.ToggleCrouchCollisionShape();
        player.SlideCooldownTimer.Start();
    }
}
