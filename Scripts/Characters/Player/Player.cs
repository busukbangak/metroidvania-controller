using Godot;
using System;

public class Player : KinematicBody2D
{

    [Signal]
    public delegate void IsGrounded(bool isGrounded);

    [Signal]
    public delegate void LookAheadVerticalDirection(int direction);

    [Signal]
    public delegate void DirectionChanged(int direction);

    [Export]
    public int Health = 2;

    [Export]
    public float CrouchSpeed = 1.5f * Constants.Units.UNIT_SIZE;

    [Export]
    public float WalkSpeed = 2f * Constants.Units.UNIT_SIZE;

    [Export]
    public float RunSpeed = 6f * Constants.Units.UNIT_SIZE;

    [Export]
    public float SlideSpeed = 12f * Constants.Units.UNIT_SIZE;

    [Export]
    public float DodgeSpeed = 15f * Constants.Units.UNIT_SIZE;

    [Export]
    public float MaxJumpHeight = 10f * Constants.Units.UNIT_SIZE;

    [Export]
    public float MinJumpHeight = 3f * Constants.Units.UNIT_SIZE;

    [Export]
    public float JumpBufferHeightBonus = 1.2f;

    [Export]
    public float WallSlideSpeed = 5f * Constants.Units.UNIT_SIZE;

    [Export]
    public float Gravity = 20f * Constants.Units.UNIT_SIZE;

    [Export]
    public int MaxExtraJumps = 1;

    [Export]
    public PackedScene FootStepDust;

    [Export]
    public PackedScene Arrow;

    public int ExtraJumpsLeft;

    public Vector2 Velocity = new Vector2();

    public AnimatedSprite AnimatedSprite;

    public StateMachine StateMachine;

    public Timer CoyoteTimer;

    public Timer JumpBuffer;

    public Timer LookAheadTimer;

    public Timer DashTimer;

    public Timer SlideTimer;

    public Timer DodgeTimer;

    public Timer DodgeCoolDownTimer;

    public Timer DashCooldownTimer;

    public Timer SlideCooldownTimer;

    public Timer BowAttackTimer;

    public Timer ComboAttackTimer;

    public Timer HurtTimer;

    public bool WasOnGround = false;

    public RayCast2D WallRaycast;

    public RayCast2D LedgeRaycast;

    public RayCast2D CeilingRaycast;

    public CollisionShape2D StandCollision;

    public CollisionShape2D CrouchAndSlideCollision;

    public Position2D HitboxPivot;


    public override void _Ready()
    {

        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        StandCollision = GetNode<CollisionShape2D>("StandCollision");
        CrouchAndSlideCollision = GetNode<CollisionShape2D>("CrouchAndSlideCollision");

        CoyoteTimer = GetNode<Timer>("Timers/CoyoteTimer");
        JumpBuffer = GetNode<Timer>("Timers/JumpBuffer");
        LookAheadTimer = GetNode<Timer>("Timers/LookAheadTimer");
        DashTimer = GetNode<Timer>("Timers/DashTimer");
        SlideTimer = GetNode<Timer>("Timers/SlideTimer");
        DodgeTimer = GetNode<Timer>("Timers/DodgeTimer");
        DodgeCoolDownTimer = GetNode<Timer>("Timers/DodgeCooldownTimer");
        DashCooldownTimer = GetNode<Timer>("Timers/DashCooldownTimer");
        SlideCooldownTimer = GetNode<Timer>("Timers/SlideCooldownTimer");
        BowAttackTimer = GetNode<Timer>("Timers/BowAttackTimer");
        ComboAttackTimer = GetNode<Timer>("Timers/ComboAttackTimer");
        HurtTimer = GetNode<Timer>("Timers/HurtTimer");

        WallRaycast = GetNode<RayCast2D>("Raycasts/WallRaycast");
        LedgeRaycast = GetNode<RayCast2D>("Raycasts/LedgeRaycast");
        CeilingRaycast = GetNode<RayCast2D>("Raycasts/CeilingRaycast");

        HitboxPivot = GetNode<Position2D>("HitboxPivot");

        StateMachine = new StateMachine(this);
        StateMachine.Add("idle", new PlayerIdleState(StateMachine));
        StateMachine.Add("jump", new PlayerJumpState(StateMachine));
        StateMachine.Add("air_jump", new PlayerAirJumpState(StateMachine));
        StateMachine.Add("fall", new PlayerFallState(StateMachine));
        StateMachine.Add("walk", new PlayerWalkState(StateMachine));
        StateMachine.Add("run", new PlayerRunState(StateMachine));
        StateMachine.Add("wall_slide", new PlayerWallSlideState(StateMachine));
        StateMachine.Add("wall_jump", new PlayerWallJumpState(StateMachine));
        StateMachine.Add("hanging", new PlayerHangingState(StateMachine));
        StateMachine.Add("dash", new PlayerDashState(StateMachine));
        StateMachine.Add("dodge", new PlayerDodgeState(StateMachine));
        StateMachine.Add("crouch", new PlayerCrouchState(StateMachine));
        StateMachine.Add("slide", new PlayerSlideState(StateMachine));
        StateMachine.Add("bow_attack", new PlayerBowAttackState(StateMachine));
        StateMachine.Add("air_bow_attack", new PlayerAirBowAttackState(StateMachine));
        StateMachine.Add("sword_attack", new PlayerSwordAttackState(StateMachine));
        StateMachine.Add("air_sword_attack", new PlayerAirSwordAttackState(StateMachine));
        StateMachine.Add("block", new PlayerBlockState(StateMachine));

        StateMachine.Change("idle");


        DebugManager.Add("state", this, nameof(ToStringPlayerState), true);
        DebugManager.Add("ceiling", this, nameof(IsCollidingWithCeiling), true);
    }

    public override void _PhysicsProcess(float delta)
    {
        ApplyGravity(delta);
        StateMachine.Update(delta);
        Move();
    }

    public override void _Input(InputEvent @event)
    {
        StateMachine.HandleInput(@event);
    }

    public void OnAnimatedSpriteAnimationFinished()
    {
        StateMachine.HandleSignal(SignalType.AnimationFinished);
    }

    public void OnSwordHitboxAreaEntered(Area2D area)
    {
        StateMachine.HandleSignal(SignalType.Hit);
    }

    public void OnHurtboxAreaEntered(Area2D area)
    {
        StateMachine.HandleSignal(SignalType.Hurt);
    }

    public void OnHurtTimerTimeout()
    {
        Modulate = new Color(1, 1, 1, 1);
    }

    public int GetInputDirection()
    {
        return -(Input.IsActionPressed("ui_left") ? 1 : 0) + (Input.IsActionPressed("ui_right") ? 1 : 0);
    }

    public int GetPlayerDirection()
    {
        return AnimatedSprite.FlipH ? -1 : 1;
    }

    public void Move()
    {
        Velocity = MoveAndSlide(Velocity, Vector2.Up);
    }

    public void ToggleCrouchCollisionShape()
    {
        StandCollision.Disabled = !StandCollision.Disabled;
        CrouchAndSlideCollision.Disabled = !CrouchAndSlideCollision.Disabled;
        CeilingRaycast.Position = new Vector2(CeilingRaycast.Position.x, CeilingRaycast.Position.y * -1);
        WallRaycast.Position = new Vector2(WallRaycast.Position.x, WallRaycast.Position.y * -1);
        LedgeRaycast.Position = new Vector2(LedgeRaycast.Position.x, LedgeRaycast.Position.y * -1);
    }

    public void EmitFootDust()
    {
        Particles2D footStepDustEmitter = (Particles2D)FootStepDust.Instance();
        AddChild(footStepDustEmitter);
        if (StateMachine.CurrentState is PlayerJumpState)
        {
            footStepDustEmitter.GlobalPosition = new Vector2(GlobalPosition.x - 5 * GetInputDirection(), GlobalPosition.y + 22);
        }
        else if (StateMachine.CurrentState is PlayerWallJumpState)
        {
            footStepDustEmitter.GlobalPosition = new Vector2(GlobalPosition.x + 10 * GetInputDirection(), GlobalPosition.y + 17);
            footStepDustEmitter.GlobalRotation = 90f;
        }
        else
        {
            footStepDustEmitter.GlobalPosition = new Vector2(GlobalPosition.x - 5 * GetInputDirection(), GlobalPosition.y + 17);
        }

        footStepDustEmitter.Emitting = true;
    }

    public void ShootArrow()
    {
        Arrow arrow = (Arrow)Arrow.Instance();
        Connect(nameof(DirectionChanged), arrow, "OnPlayerDirectionChanged");
        EmitSignal(nameof(DirectionChanged), GetPlayerDirection());
        GetParent().AddChild(arrow);
        arrow.GlobalPosition = new Vector2(GlobalPosition.x + 7, GlobalPosition.y + 3);
    }

    public void ApplyGravity(float delta)
    {
        // disable gravity for coyote time
        if (CoyoteTimer.IsStopped())
        {
            Velocity.y += Gravity * delta;
        }
    }

    public float GetLerpWeight()
    {
        var direction = GetInputDirection();
        if (IsOnFloor() || !CoyoteTimer.IsStopped())
        {
            return 0.15f;
        }
        else
        {
            if (direction == 0)
            {
                return 0.02f;
            }
            else if (direction == Mathf.Sign(Velocity.x) && Mathf.Abs(Velocity.x) > RunSpeed)
            {
                return 0.0f;
            }
            else
            {
                return 0.1f;
            }
        }
    }

    public bool IsCollidingWithWall()
    {
        return WallRaycast.IsColliding() ? true : false;
    }

    public bool IsOnLedge()
    {
        return WallRaycast.IsColliding() && !LedgeRaycast.IsColliding() ? true : false;
    }

    public bool IsCollidingWithCeiling()
    {
        return CeilingRaycast.IsColliding() ? true : false;
    }

    public void UpdateBodyDirection()
    {
        var direction = GetInputDirection();
        AnimatedSprite.FlipH = direction != 0 ? direction == -1 ? true : false : AnimatedSprite.FlipH;

        if (AnimatedSprite.FlipH)
        {
            WallRaycast.CastTo = new Vector2(-Mathf.Abs(WallRaycast.CastTo.x), WallRaycast.CastTo.y);
            LedgeRaycast.CastTo = new Vector2(-Mathf.Abs(LedgeRaycast.CastTo.x), LedgeRaycast.CastTo.y);
            HitboxPivot.RotationDegrees = 180;
        }
        else
        {
            WallRaycast.CastTo = new Vector2(Mathf.Abs(WallRaycast.CastTo.x), WallRaycast.CastTo.y);
            LedgeRaycast.CastTo = new Vector2(Mathf.Abs(LedgeRaycast.CastTo.x), LedgeRaycast.CastTo.y);
            HitboxPivot.Rotation = 0;
        }
    }

    public void Hurt(int damage)
    {
        if (HurtTimer.IsStopped())
        {
            HurtTimer.Start();
            Modulate = new Color(10, 10, 10, 10);
            SetHealth(Health - damage);
        }
    }

    public void SetHealth(int health)
    {

        if (health > 0)
        {
            Health = health;
        }
        else
        {
            StateMachine.HandleSignal(SignalType.Dead);
        }

    }

    public bool IsPlayerOnFloor()
    {
        return IsOnFloor();
    }

    public string ToStringPlayerState()
    {
        return StateMachine.CurrentState.ToString();
    }

}