using Godot;
using System;

public class PlayerAirSwordAttackState : State
{
    public bool AnimationFinished = false;

    private int _swordAttackState = 0;

    public PlayerAirSwordAttackState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        player.Velocity.x = Mathf.Lerp(player.Velocity.x, _swordAttackState != 2 ? player.WalkSpeed * direction : 0, player.GetLerpWeight());

        if (_swordAttackState == 2)
        {
            if (player.IsOnFloor() && AnimationFinished)
            {
                _stateMachine.Change("air_sword_attack");
            }
            return;
        }

        if (AnimationFinished)
        {
            if (direction != 0 && player.IsOnFloor())
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
    }

    public override void HandleSignal(SignalType signal)
    {
        if (signal == SignalType.AnimationFinished)
        {
            AnimationFinished = true;
        }
        if (signal == SignalType.Hit)
        {
            _stateMachine.Change("jump");
        }
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.HitboxPivot.GetNode<CollisionShape2D>("SwordHitbox/CollisionShape2D").Disabled = false;

        if (_swordAttackState == 3)
        {
            player.AnimatedSprite.Play("air_sword_attack_4");
            player.HitboxPivot.RotationDegrees = 90;
            return;
        }



        if (!player.ComboAttackTimer.IsStopped())
        {
            if (_swordAttackState == 1)
            {
                player.AnimatedSprite.Play("air_sword_attack_2");
            }
            else if (_swordAttackState == 2)
            {
                player.AnimatedSprite.Play("air_sword_attack_3");
                player.HitboxPivot.RotationDegrees = 90;
            }
        }
        else
        {
            _swordAttackState = 0;
            player.AnimatedSprite.Play("air_sword_attack_1");
        }
    }


    public override void HandleInput(InputEvent @event)
    {
        Player player = (Player)_stateMachine.Parent;
        if (@event.IsActionPressed("ui_down"))
        {
            _swordAttackState = 2;
            player.AnimatedSprite.Play("air_sword_attack_3");
            player.HitboxPivot.RotationDegrees = 90;
            return;
        }
    }




    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        AnimationFinished = false;
        player.HitboxPivot.GetNode<CollisionShape2D>("SwordHitbox/CollisionShape2D").SetDeferred("disabled", true);
        player.UpdateBodyDirection();

        if (_swordAttackState >= 3)
        {
            _swordAttackState = 0;
            player.ComboAttackTimer.Stop();
        }
        else
        {
            _swordAttackState++;
            player.ComboAttackTimer.Start();
        }
    }
}
