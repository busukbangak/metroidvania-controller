using Godot;
using System;

public class ExitScreen : Node
{
    public override void _Ready()
    {
        GetTree().Quit();
    }
}
