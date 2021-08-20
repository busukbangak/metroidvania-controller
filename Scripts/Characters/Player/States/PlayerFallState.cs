using Godot;
using System;

public class PlayerFallState : State
{

    public PlayerFallState(StateMachine stateMaschine) : base(stateMaschine)
    {

    }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (Input.IsActionPressed("bow_attack"))
        {
            _stateMachine.Change("air_bow_attack");
            return;
        }

        if (Input.IsActionPressed("sword_attack"))
        {
            _stateMachine.Change("air_sword_attack");
            return;
        }


        if (Input.IsActionJustPressed("dash") && player.DashCooldownTimer.IsStopped())
        {
            _stateMachine.Change("dash");
            return;
        }

        if (player.IsOnLedge() && direction == player.GetPlayerDirection() && Input.IsActionPressed("ui_up"))
        {
            _stateMachine.Change("hanging");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            player.JumpBuffer.Start();
        }

        // Jump buffer jump
        if (player.IsOnFloor() && !player.JumpBuffer.IsStopped())
        {
            player.JumpBuffer.Stop();
            _stateMachine.Change("jump", new object[] { true });
            return;
        }

        if (player.IsOnFloor())
        {
            if (player.Velocity.x > player.WalkSpeed)
            {
                _stateMachine.Change("run");
                return;
            }
            else if (player.Velocity.x > 0)
            {
                _stateMachine.Change("walk");
                return;
            }
            else
            {
                _stateMachine.Change("idle");
                return;
            }
        }

        if (player.IsCollidingWithWall() && direction == player.GetPlayerDirection())
        {
            _stateMachine.Change("wall_slide");
            return;
        }

        // Coyote time
        if (Input.IsActionJustPressed("jump") && !player.CoyoteTimer.IsStopped() && player.WasOnGround)
        {
            player.CoyoteTimer.Stop();
            _stateMachine.Change("jump");
            return;
        }

        // Air jump
        if (Input.IsActionJustPressed("jump") && player.ExtraJumpsLeft > 0)
        {
            _stateMachine.Change("air_jump");
            return;
        }

        // Playing is now really falling
        if (player.CoyoteTimer.IsStopped())
        {
            player.AnimatedSprite.Play("fall");
        }

        bool isRunning = Input.IsActionPressed("run");
        player.UpdateBodyDirection();
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, (isRunning ? player.RunSpeed : player.WalkSpeed) * direction, player.GetLerpWeight());
    }

    public override void HandleSignal(SignalType signal)
    {
   
    }

    public override void Enter(params object[] args)
    {

        Player player = (Player)_stateMachine.Parent;

        // player was on ground before falling
        if (player.WasOnGround)
        {
            player.CoyoteTimer.Start();
            player.Velocity.y = 0;
            player.AnimatedSprite.Play("walk");
            return;
        }

        player.AnimatedSprite.Play("fall");

    }

    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = false;

        player.EmitSignal(nameof(Player.IsGrounded), false);
    }
}
