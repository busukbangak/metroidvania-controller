using Godot;
using System;

public class PlayerHangingState : State
{

    public PlayerHangingState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if(!player.IsCollidingWithWall()) {
            _stateMachine.Change("fall");
            return;
        }

        if (!Input.IsActionPressed("ui_up") && direction == player.GetPlayerDirection())
        {
            _stateMachine.Change("wall_slide");
            return;
        }

        if (direction != player.GetPlayerDirection()  && !Input.IsActionPressed("ui_up") )
        {
            _stateMachine.Change("fall");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            _stateMachine.Change("wall_jump");
            return;
        }

        player.Velocity.y = 0;

    }

    public override void HandleSignal(SignalType signal)
    {
  
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.ExtraJumpsLeft = player.MaxExtraJumps;
        player.AnimatedSprite.Play("hanging");
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;
    }
}
