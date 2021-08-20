using Godot;
using System;

public class MainMenu : CanvasLayer
{
    private int _currentSelection = 0;

    private Label _selectorOne;

    private Label _selectorTwo;

    private Label _selectorThree;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _selectorOne = GetNode<Label>("CenterContainer/VBoxContainer/CenterContainer2/HBoxContainer/Selector");
        _selectorTwo = GetNode<Label>("CenterContainer/VBoxContainer/CenterContainer3/HBoxContainer/Selector");
        _selectorThree = GetNode<Label>("CenterContainer/VBoxContainer/CenterContainer4/HBoxContainer/Selector");

        SetCurrentSelection();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_down") && _currentSelection < 2)
        {
            _currentSelection += 1;
            SetCurrentSelection();
        }
        else if (Input.IsActionJustPressed("ui_up") && _currentSelection > 0)
        {
            _currentSelection -= 1;
            SetCurrentSelection();
        }
        else if (Input.IsActionJustPressed("ui_accept"))
        {
            HandleAcceptedSelection(_currentSelection);
        }
    }

    private void SetCurrentSelection()
    {
        _selectorOne.Text = "";
        _selectorTwo.Text = "";
        _selectorThree.Text = "";

        switch (_currentSelection)
        {
            case 0: _selectorOne.Text = ">"; break;
            case 1: _selectorTwo.Text = ">"; break;
            case 2: _selectorThree.Text = ">"; break;
        }

    }

    private void HandleAcceptedSelection(int _currentSelection)
    {
        switch (_currentSelection)
        {
            case 0:
                UIManager.Add("pause", Constants.Screens.PAUSE_MENU);
                WorldManager.ChangeWorldSpace(Constants.Maps.GRASS_1);
                break;
            case 1:
                _selectorTwo.Text = ">";
                break;
            case 2:
                WorldManager.ChangeWorldSpace(Constants.Screens.QUIT);
                break;
        }
    }
}