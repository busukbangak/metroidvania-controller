using Godot;
using System;

public class PlayerBlockState : State
{

    public PlayerBlockState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if(!Input.IsActionPressed("block")) {
            _stateMachine.Change("idle");
        }
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, 0, player.GetLerpWeight());
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.AnimatedSprite.Play("block");
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
    }
}
