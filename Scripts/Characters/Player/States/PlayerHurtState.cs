using Godot;
using System;

public class PlayerHurtState : State
{

    public PlayerHurtState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();


        if (!player.HurtTimer.IsStopped())
        {
            _stateMachine.Change("idle");
            return;
        }

        if (Input.IsActionPressed("block"))
        {
            _stateMachine.Change("block");
            return;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            _stateMachine.Change("crouch");
            return;
        }

        if (direction != 0)
        {
            if (Input.IsActionPressed("run"))
            {
                _stateMachine.Change("run");
                return;
            }
            _stateMachine.Change("walk");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            _stateMachine.Change("jump");
            return;
        }

        if (!player.IsOnFloor())
        {
            _stateMachine.Change("fall");
            return;
        }

        if (Input.IsActionPressed("bow_attack"))
        {
            _stateMachine.Change("bow_attack");
            return;
        }

        if (Input.IsActionPressed("sword_attack"))
        {
            _stateMachine.Change("sword_attack");
            return;
        }

        if (Mathf.Abs(player.Velocity.x) > 1)
        {
            player.Velocity.x = Mathf.Lerp(player.Velocity.x, 0, player.GetLerpWeight());
        }
        else
        {
            player.Velocity.x = 0;
        }

    }

    public override void HandleSignal(SignalType signal)
    {
     
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.Hurt(1);
        player.Velocity.x = player.RunSpeed * 3f * player.GetPlayerDirection() * -1;
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;

    }
}
