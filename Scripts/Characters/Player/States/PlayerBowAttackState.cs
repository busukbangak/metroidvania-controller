using Godot;
using System;

public class PlayerBowAttackState : State
{
    public PlayerBowAttackState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (player.BowAttackTimer.IsStopped())
        {
            _stateMachine.Change("idle");
        }

        player.Velocity.x = Mathf.Lerp(player.Velocity.x, 0, player.GetLerpWeight());

    }

    public override void HandleSignal(SignalType signal)
    {
 
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.AnimatedSprite.Play("bow_attack");
        player.BowAttackTimer.Start();


    }

    public override void Exit()
    {
        
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;
        
        player.ShootArrow();
    }
}
