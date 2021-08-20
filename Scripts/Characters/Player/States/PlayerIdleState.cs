using Godot;
using System;

public class PlayerIdleState : State
{

    private bool _isLookingAhead = false;

    private Vector2 _previousCameraPosition;

    public PlayerIdleState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        if(Input.IsActionPressed("block")) {
            _stateMachine.Change("block");
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

        if (Input.IsActionJustPressed("ui_up"))
        {
            player.LookAheadTimer.Start();
        }

        if (Input.IsActionPressed("ui_up") && player.LookAheadTimer.IsStopped() && !_isLookingAhead)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), -1);
            _isLookingAhead = true;
        }

        if (Input.IsActionJustReleased("ui_up") && _isLookingAhead)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), 0);
            _isLookingAhead = false;
        }

        if(Input.IsActionPressed("bow_attack")) {
            _stateMachine.Change("bow_attack");
            return;
        }

        if(Input.IsActionPressed("sword_attack")) {
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
        player.ExtraJumpsLeft = player.MaxExtraJumps;
        player.AnimatedSprite.Play("idle");

        player.EmitSignal(nameof(Player.IsGrounded), true);

        // Start timer to keep cooldown, if up key is still pressed from previous state
        player.LookAheadTimer.Start();

    }

    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        player.WasOnGround = true;

        player.LookAheadTimer.Stop();

        if (_isLookingAhead)
        {
            player.EmitSignal(nameof(Player.LookAheadVerticalDirection), 0);
            _isLookingAhead = false;
        }
    }
}
