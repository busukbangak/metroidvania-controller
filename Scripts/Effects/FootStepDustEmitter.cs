using Godot;
using System;

public class FootStepDustEmitter : Particles2D
{

    SceneTreeTimer alive;

    public override void _Ready()
    {
        var timer = GetTree().CreateTimer(1.5f);
        timer.Connect("timeout", this, "OnTimeOut");

    }

    public void OnTimeOut()
    {
        QueueFree();
    }

}
