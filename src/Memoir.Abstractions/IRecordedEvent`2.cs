namespace Memoir
{
    public interface IRecordedEvent<TEvent,TState> : IRecordedEvent<TState> where TEvent : IEvent<TState>
    {
        new TEvent Data { get; }
    }
}
