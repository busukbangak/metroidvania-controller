using Godot;
using System;

public class PlayerWallSlideState : State
{
    public PlayerWallSlideState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (player.IsOnFloor() || direction != player.GetPlayerDirection() || !player.IsCollidingWithWall())
        {
            _stateMachine.Change("fall");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            _stateMachine.Change("wall_jump");
            return;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            player.Velocity.y = player.WallSlideSpeed * 1.75f;
        }
        else if (Input.IsActionPressed("ui_up"))
        {
            player.Velocity.y = 0;
        }
        else
        {
            player.Velocity.y = player.WallSlideSpeed;
        }
    }

    public override void HandleSignal(SignalType signal)
    {
     
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.ExtraJumpsLeft = player.MaxExtraJumps;

        player.AnimatedSprite.Play("wall_slide");

        player.EmitSignal(nameof(Player.IsGrounded), false);
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = false;

    }
}
