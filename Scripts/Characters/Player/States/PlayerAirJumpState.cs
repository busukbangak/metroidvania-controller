using Godot;
using System;

public class PlayerAirJumpState : State
{
    public PlayerAirJumpState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;

        if (player.IsOnFloor())
        {
            _stateMachine.Change("idle");
            return;
        }

        if (player.Velocity.y >= 0)
        {
            _stateMachine.Change("fall");
            return;
        }

        if (Input.IsActionJustReleased("jump") && player.Velocity.y < -player.MinJumpHeight)
        {
            player.Velocity.y = -player.MinJumpHeight;
        }

        if (Input.IsActionJustPressed("dash") && player.DashCooldownTimer.IsStopped())
        {
            _stateMachine.Change("dash");
            return;
        }
    }

    public override void HandleInput(InputEvent @event)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (@event.IsActionPressed("bow_attack"))
        {
            _stateMachine.Change("air_bow_attack");
            return;
        }

        if (@event.IsActionPressed("sword_attack"))
        {
            _stateMachine.Change("air_sword_attack");
            return;
        }

        if (player.IsOnLedge() && direction != 0 && @event.IsActionPressed("ui_up"))
        {
            _stateMachine.Change("hanging");
            return;
        }

        if (player.IsCollidingWithWall() && direction != 0 && player.Velocity.y < -player.MaxJumpHeight)
        {
            _stateMachine.Change("wall_slide");
            return;
        }


        bool isRunning = @event.IsActionPressed("run");
        player.UpdateBodyDirection();
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, (isRunning ? player.RunSpeed : player.WalkSpeed) * direction, player.GetLerpWeight());
    }

    public override void HandleSignal(SignalType signal)
    {

    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.Velocity.y = -player.MaxJumpHeight;
        player.ExtraJumpsLeft--;
        player.AnimatedSprite.Play("somersault");

        player.EmitSignal(nameof(Player.IsGrounded), false);
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = false;
    }
}
