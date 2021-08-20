using Godot;
using System.Collections.Generic;

public enum SignalType
{
    AnimationFinished,
    Hurt,
    Hit,
    HurtTimerTimeout,
    Dead
}

public class StateMachine : Node
{
    private Dictionary<string, State> _stateDict = new Dictionary<string, State>();

    public State CurrentState;

    public Node Parent;

    public StateMachine(Node parent)
    {
        Parent = parent;
    }

    public void Update(float delta)
    {
        CurrentState?.Update(delta);
    }

    public void HandleInput(InputEvent @event)
    {
        CurrentState?.HandleInput(@event);
    }

    public void HandleSignal(SignalType signal) {
        CurrentState?.HandleSignal(signal);
    }

    public void Change(string id, params object[] args)
    {
        CurrentState?.Exit();
        State nextState = _stateDict[id];
        nextState.Enter(args);
        CurrentState = nextState;

    }

    public void Add(string id, State state)
    {
        _stateDict.Add(id, state);
    }

    public void Remove(string id)
    {
        _stateDict.Remove(id);
    }

    public State Get(string id)
    {
        return _stateDict[id];
    }

    public void Clear()
    {
        _stateDict.Clear();
    }
}