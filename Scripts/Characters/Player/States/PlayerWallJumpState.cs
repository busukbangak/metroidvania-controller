using Godot;
using System;

public class PlayerWallJumpState : State
{

    private bool _emittedFootDust = false;
    public PlayerWallJumpState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (!_emittedFootDust)
        {
            player.EmitFootDust();
            _emittedFootDust = true;
        }

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

        if (player.IsOnLedge() && direction != 0 && Input.IsActionPressed("ui_up"))
        {
            _stateMachine.Change("hanging");
            return;
        }

        if (player.IsCollidingWithWall() && direction != 0 && player.Velocity.y < -player.MaxJumpHeight)
        {
            _stateMachine.Change("wall_slide");
            return;
        }

        if (Input.IsActionJustPressed("jump") && player.ExtraJumpsLeft != 0)
        {
            _stateMachine.Change("air_jump");
            return;
        }

        if (Input.IsActionJustReleased("jump") && player.Velocity.y < -player.MinJumpHeight)
        {
            player.Velocity.y = -player.MinJumpHeight;
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
        int direction = player.GetInputDirection();
        player.Velocity.x = player.RunSpeed * 2 * -direction;
        player.Velocity.y = -player.MaxJumpHeight;
        player.AnimatedSprite.Play("jump");
        player.EmitSignal(nameof(Player.IsGrounded), false);
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = false;
        _emittedFootDust = false;
    }
}
