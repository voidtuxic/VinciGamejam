namespace Void.States
{
    public interface IState
    {
        StateId StateId { get; }
        void Enter();
        void Update();
        void Exit();
    }
}
