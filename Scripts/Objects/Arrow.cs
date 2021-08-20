using Godot;
using System;

public class Arrow : Node2D
{
    [Export]
    public float mass = 0.1f;

    [Export]
    public Vector2 Velocity = new Vector2(30 * Constants.Units.UNIT_SIZE, -16);

    private bool _isFlying = true;

    private bool _isSignalAlreadyRecieved = false;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_isFlying)
        {
            Velocity += Vector2.Down * mass;
            Position += Velocity * delta;
            Rotation = Velocity.Angle();
        }
    }

    public void OnPlayerDirectionChanged(int direction)
    {
        if (!_isSignalAlreadyRecieved)
        {
            _isSignalAlreadyRecieved = true;
            Velocity = new Vector2(Velocity.x * direction, Velocity.y);
        }
    }

    public void OnHitboxBodyEntered(Node body)
    {
        Velocity = Vector2.Zero;
        _isFlying = false;
    }

    public void OnHitboxAreaEntered(Area2D area)
    {
        Velocity = Vector2.Zero;
        _isFlying = false;
    }

    public void OnHitboxAreaExit(Area2D area)
    {
        _isFlying = true;
    }

    public void OnDespawnTimerTimeout()
    {
        QueueFree();
    }
}
