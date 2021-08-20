using Godot;
using System;

public class PlayerSwordAttackState : State
{
    public bool AnimationFinished = false;

    private int _swordAttackState = 0;

    public PlayerSwordAttackState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {
        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

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
        player.Velocity.x = Mathf.Lerp(player.Velocity.x, 0 * player.GetPlayerDirection(), player.GetLerpWeight());

    }

    public override void HandleSignal(SignalType signal)
    {
        if (signal == SignalType.AnimationFinished)
        {
            AnimationFinished = true;
        }
        if (signal == SignalType.Dead)
        {
            _stateMachine.Change("dead");
        }
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        player.HitboxPivot.GetNode<CollisionShape2D>("SwordHitbox/CollisionShape2D").Disabled = false;

        if (!player.ComboAttackTimer.IsStopped())
        {
            if (_swordAttackState == 1)
            {
                player.AnimatedSprite.Play("sword_attack_2");
            }
            else
            {
                player.AnimatedSprite.Play("sword_attack_3");
            }
        }
        else
        {
            _swordAttackState = 0;
            player.AnimatedSprite.Play("sword_attack_1");
        }
    }



    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        AnimationFinished = false;
        player.HitboxPivot.GetNode<CollisionShape2D>("SwordHitbox/CollisionShape2D").Disabled = true;


        if (_swordAttackState >= 2)
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
