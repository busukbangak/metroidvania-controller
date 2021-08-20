using Godot;
using System;

public class PlayerCamera : Camera2D
{

    private Vector2 _previousCameraPosition;

    private Vector2 _previousCameraPosition2;

    private int horizontalDirection = 0;

    private float _lookAheadFactor = 0.2f;

    private Tween _tween;

    private float _shiftDuration = 1.0f;

    public override void _Ready()
    {
        _tween = GetNode<Tween>("Tween");
        _previousCameraPosition = GetCameraPosition();
        _previousCameraPosition2 = GetCameraPosition();

    }

    public override void _Process(float delta)
    {
        PlayerLookAheadHorizontalDirection();
        _previousCameraPosition = GetCameraPosition();
    }

    public void OnPlayerLookAheadVerticalDirection(int verticalDirection)
    {
        if (verticalDirection == 0)
        {
            GlobalPosition = _previousCameraPosition2;
        }
        else if (verticalDirection == -1)
        {
            _previousCameraPosition2 = GlobalPosition;
            GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y - 50);
        }
        else
        {
            _previousCameraPosition2 = GlobalPosition;
            GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y + 50);
        }
    }

    public void OnPlayerIsGrounded(bool isGrounded)
    {
        DragMarginVEnabled = !isGrounded;
    }

    public void PlayerLookAheadHorizontalDirection()
    {
        var newDirection = Mathf.Sign(GetCameraPosition().x - _previousCameraPosition.x);
        if (newDirection != 0 && horizontalDirection != newDirection)
        {
            horizontalDirection = newDirection;
            var targetOffset = GetViewportRect().Size.x * _lookAheadFactor * horizontalDirection;

            _tween.InterpolateProperty(this, "position:x", Position.x, targetOffset, _shiftDuration, Tween.TransitionType.Sine, Tween.EaseType.Out);
            _tween.Start();
        }
    }


}
