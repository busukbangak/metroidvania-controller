using Godot;
using System;

public class PauseMenu : Node
{
    public override void _Process(float delta)
    {

        if(Input.IsActionJustPressed("ui_cancel")) 
        {
            bool newPauseState = !GetTree().Paused;
            GetTree().Paused = newPauseState;
            GetNode<Control>("PauseMenuContainer").Visible = newPauseState;
        }
    }
}
