using Godot;

public abstract class State : Node
{
    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public abstract void Update(float delta);

    public virtual void HandleInput(InputEvent @event)
    {
        return;
    }

    public virtual void HandleSignal(SignalType signal)
    {
        return;
    }

    public abstract void Enter(params object[] args);

    public abstract void Exit();

    public override string ToString()
    {
        return this.GetType().Name;
    }

}
