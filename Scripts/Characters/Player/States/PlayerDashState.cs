using Godot;
using System;

public class PlayerDashState : State
{

    public PlayerDashState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (player.DashTimer.IsStopped())
        {
            _stateMachine.Change("fall");
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

        player.Velocity.x = player.RunSpeed * 3f * player.GetPlayerDirection();

        player.AnimatedSprite.Play("dash");
        player.DashTimer.Start();

    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.Velocity.x = player.RunSpeed * player.GetPlayerDirection();
        player.WasOnGround = false;

        player.DashCooldownTimer.Start();
    }
}
