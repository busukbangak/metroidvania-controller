using Godot;
using System;

public class Slime : KinematicBody2D
{

    public int Health = 3;

    private Timer _hurtTimer;

    private AnimatedSprite _animatedSprite;

    private int _moveState = 1;

    private Vector2 _velocity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _hurtTimer = GetNode<Timer>("HurtTimer");
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _PhysicsProcess(float delta)
    {

        switch (_moveState)
        {
            case -1:
                _velocity.x = 0;
                _animatedSprite.Play("die");
                break;
            case 0:
                _velocity.x = 0;
                _animatedSprite.Play("idle");
                break;
            case 1:
                _velocity.x = 50;
                _animatedSprite.FlipH = true;
                _animatedSprite.Play("move");
                break;
            case 2:
                _velocity.x = -50;
                _animatedSprite.FlipH = false;
                _animatedSprite.Play("move");
                break;
            case 3:
                _velocity.x = 0;
                _animatedSprite.Play("hurt");
                break;
        }

        Mathf.Lerp(_velocity.x, 0, 0.2f);
        _velocity.y = (int)ProjectSettings.GetSetting("physics/2d/default_gravity");
        _velocity = MoveAndSlide(_velocity, Vector2.Up);
    }

    public void OnMoveTimerTimeout()
    {
        _moveState = (int)Mathf.Floor((float)GD.RandRange(0, 3));
    }

    public void OnHitboxAreaEntered(Area2D area)
    {
        GD.Print(area.GetParent().Name);
    }

    public void OnHurtboxAreaEntered(Area2D area)
    {
        GD.Print(area.GetParent().Name);
        if (!_hurtTimer.IsStopped())
        {
            return;
        }

        Health--;
        _hurtTimer.Start();

        if (Health > 0)
        {
            var shaderMaterial = (ShaderMaterial)_animatedSprite.Material;
            shaderMaterial.SetShaderParam("hit_opacity", 0.5f);
            _moveState = 3;
        }
        else
        {
            _moveState = -1;
        }
    }

    public void OnHurtTimerTimeout()
    {
        var shaderMaterial = (ShaderMaterial)_animatedSprite.Material;
        shaderMaterial.SetShaderParam("hit_opacity", 0);
    }

    public void OnAnimatedSpriteAnimationFinished()
    {
        var currentAnimation = _animatedSprite.Animation;
        if (currentAnimation == "die")
        {
            QueueFree();
        }
        else if (currentAnimation == "hurt")
        {
            _moveState = (int)Mathf.Floor((float)GD.RandRange(0, 3));
        }

    }
}
