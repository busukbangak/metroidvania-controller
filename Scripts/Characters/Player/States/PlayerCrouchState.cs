using Godot;
using System;

public class PlayerCrouchState : State
{

    bool _isLookingAhead = false;

    bool _playerWasMoving = false;

    public PlayerCrouchState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if (direction != 0)
        {
            _playerWasMoving = true;
        }

        if (Input.IsActionJustPressed("ui_down") && direction == 0 || Input.IsActionJustPressed("ui_up") && direction == 0)
        {
            player.LookAheadTimer.Start();
            // Set it to false again, incase players head is at ceiling and he wants to look down
            _playerWasMoving = false;
        }

        if (Input.IsActionPressed("ui_down") && player.LookAheadTimer.IsStopped() && !_isLookingAhead && direction == 0 && !_playerWasMoving)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), 1);
            _isLookingAhead = true;
        }

        if (Input.IsActionJustReleased("ui_down") && _isLookingAhead || direction != 0 && _isLookingAhead)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), 0);
            _isLookingAhead = false;
        }

        if (Input.IsActionPressed("ui_up") && player.LookAheadTimer.IsStopped() && !_isLookingAhead && direction == 0 && !_playerWasMoving)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), -1);
            _isLookingAhead = true;
        }

        if (Input.IsActionJustReleased("ui_up") && _isLookingAhead || direction != 0 && _isLookingAhead)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), 0);
            _isLookingAhead = false;
        }

        if (Input.IsActionJustPressed("dodge") && player.DodgeCoolDownTimer.IsStopped() && direction != 0)
        {
            _stateMachine.Change("dodge");
            return;
        }

        if (!player.IsOnFloor())
        {
            _stateMachine.Change("fall");
            return;
        }

        if (!Input.IsActionPressed("ui_down") && !player.IsCollidingWithCeiling())
        {
            _stateMachine.Change("idle");
            return;
        }

        if (direction != 0)
        {
            player.UpdateBodyDirection();
            player.Velocity.x = Mathf.Lerp(player.Velocity.x, player.CrouchSpeed * direction, player.GetLerpWeight());
            player.AnimatedSprite.Play("crouch_walk");
        }
        else
        {
            player.Velocity.x = 0;
            player.AnimatedSprite.Play("crouch");
        }

    }

    public override void HandleSignal(SignalType signal)
    {

    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.AnimatedSprite.Play("crouch");
        player.ToggleCrouchCollisionShape();
        player.LookAheadTimer.Start();


    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;
        player.ToggleCrouchCollisionShape();
        player.LookAheadTimer.Stop();

        _playerWasMoving = false;

        if (_isLookingAhead)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), 0);
            _isLookingAhead = false;
        }
    }
}
