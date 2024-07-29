public abstract class State : IState
{
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}
