namespace Memoir
{
    public interface IRecordedEvent<TState> : IRecordedEvent
    {
        new IEvent<TState> Data { get; }
    }
}
