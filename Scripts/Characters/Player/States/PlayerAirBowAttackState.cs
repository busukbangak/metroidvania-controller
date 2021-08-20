using Godot;
using System;

public class PlayerAirBowAttackState : State
{
    public PlayerAirBowAttackState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

    }

    public override void HandleInput(InputEvent @event)
    {
        Player player = (Player)_stateMachine.Parent;
        var direction = player.GetInputDirection();
        if (player.BowAttackTimer.IsStopped())
        {
            if ( direction != 0 && player.IsOnFloor())
            {

                _stateMachine.Change("walk");
                player.WasOnGround = true;
            }
            else if (player.IsOnFloor())
            {
                _stateMachine.Change("idle");
                player.WasOnGround = true;
            }
            else
            {
                _stateMachine.Change("fall");
                player.WasOnGround = false;
            }
        }


        player.Velocity.x = Mathf.Lerp(player.Velocity.x, player.WalkSpeed * direction, player.GetLerpWeight());
    }

    public override void HandleSignal(SignalType signal)
    {

    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.AnimatedSprite.Play("air_bow_attack");
        player.BowAttackTimer.Start();

    }

    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = false;
        player.ShootArrow();
    }
}
