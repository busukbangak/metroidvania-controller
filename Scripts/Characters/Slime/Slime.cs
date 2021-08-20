using Godot;
using System;

public class Slime : KinematicBody2D
{

    public int Health = 10;

    private Timer _hurtTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _hurtTimer = GetNode<Timer>("HurtTimer");
    }

    public void OnHitboxAreaEntered(Area2D area)
    {
        GD.Print(area.GetParent());
    }

    public void OnHurtboxAreaEntered(Area2D area)
    {
        GD.Print(area.GetParent());
        if (!_hurtTimer.IsStopped())
        {
            return;
        }

        Health--;
        _hurtTimer.Start();

        if (Health > 0)
        {
            Modulate = new Color(10, 10, 10, 10);
        }
        else
        {
            QueueFree();
        }
    }

    public void OnHurtTimerTimeout()
    {
        Modulate = new Color(1, 1, 1, 1);
    }
}
