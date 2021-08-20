using Godot;
using System;

public class PlayerRunState : State
{
    private int _lastStep;

    public PlayerRunState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if(Input.IsActionPressed("block")) {
            _stateMachine.Change("block");
        }

        if (Input.IsActionPressed("bow_attack"))
        {
            _stateMachine.Change("bow_attack");
            return;
        }

        if(Input.IsActionPressed("sword_attack")) {
            _stateMachine.Change("sword_attack");
            return;
        }

        if (player.AnimatedSprite.Frame == 0 || player.AnimatedSprite.Frame == 3)
        {
            if (_lastStep != player.AnimatedSprite.Frame)
            {
                _lastStep = player.AnimatedSprite.Frame;
                player.EmitFootDust();
            }

        }

        if (Input.IsActionJustPressed("dash") && player.DashCooldownTimer.IsStopped())
        {
            _stateMachine.Change("dash");
            return;
        }

        if (Input.IsActionPressed("ui_down") && player.SlideCooldownTimer.IsStopped() && Mathf.Abs(player.Velocity.x) > Mathf.Abs(player.RunSpeed) - 5)
        {
            _stateMachine.Change("slide");
            return;
        }



        if (!player.IsOnFloor())
        {
            _stateMachine.Change("fall");
            return;
        }

        if (direction == 0)
        {
            _stateMachine.Change("idle");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            _stateMachine.Change("jump");
            return;
        }

        if (!Input.IsActionPressed("run"))
        {
            _stateMachine.Change("walk");
            return;
        }

        if (Input.IsActionJustPressed("dodge") && player.DodgeCoolDownTimer.IsStopped())
        {
            _stateMachine.Change("dodge");
            return;
        }

        player.UpdateBodyDirection();
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, player.RunSpeed * direction, player.GetLerpWeight());
    }

    public override void HandleSignal(SignalType signal)
    {

    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.AnimatedSprite.Play("run");
        player.EmitSignal(nameof(Player.IsGrounded), true);
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;
        _lastStep = -1;
    }
}
