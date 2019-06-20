namespace Memoir
{
    public interface IPendingEventFactory
    {
        IPendingEvent Create<TAggregate, TState>(TAggregate aggregate, IEvent<TState> @event) where TAggregate : IAggregate<TState>;
    }
}
