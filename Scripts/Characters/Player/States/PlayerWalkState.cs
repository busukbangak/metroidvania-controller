using Godot;
using System;

public class PlayerWalkState : State
{

    public PlayerWalkState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();
        
        if(Input.IsActionPressed("block")) {
            _stateMachine.Change("block");
        }

        if (Input.IsActionJustPressed("dash") && player.DashCooldownTimer.IsStopped())
        {
            _stateMachine.Change("dash");
            return;
        }
        
        if (Input.IsActionPressed("ui_down"))
        {
            _stateMachine.Change("crouch");
            return;
        }
        if (direction == 0)
        {
            _stateMachine.Change("idle");
            return;
        }

        if (Input.IsActionPressed("jump"))
        {
            _stateMachine.Change("jump");
            return;
        }

        if(Input.IsActionPressed("bow_attack")) {
            _stateMachine.Change("bow_attack");
            return;
        }

        if (Input.IsActionPressed("run"))
        {
            _stateMachine.Change("run");
            return;
        }

        if (!player.IsOnFloor())
        {
            _stateMachine.Change("fall");
            return;
        }

        if (Input.IsActionJustPressed("dodge") && player.DodgeCoolDownTimer.IsStopped())
        {
            _stateMachine.Change("dodge");
            return;
        }

        if(Input.IsActionPressed("sword_attack")) {
            _stateMachine.Change("sword_attack");
            return;
        }

        player.UpdateBodyDirection();
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, player.WalkSpeed * direction, player.GetLerpWeight());

    }

    public override void HandleSignal(SignalType signal)
    {
  
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.AnimatedSprite.Play("walk");
        player.EmitSignal(nameof(Player.IsGrounded), true);
    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;
    }

}
