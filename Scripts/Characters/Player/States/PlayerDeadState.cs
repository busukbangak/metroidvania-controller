using Godot;
using System;

public class PlayerDeadState : State
{

    public PlayerDeadState(StateMachine stateMaschine) : base(stateMaschine) { }

    public override void Update(float delta)
    {

        Player player = (Player)_stateMachine.Parent;
        int direction = player.GetInputDirection();

        GD.Print("update");
    }

    public override void Enter(params object[] args)
    {
        Player player = (Player)_stateMachine.Parent;
        GD.Print("enter");

    }
    public override void Exit()
    {
        Player player = (Player)_stateMachine.Parent;
        GD.Print("exit");
    }
}
