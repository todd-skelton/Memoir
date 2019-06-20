namespace Memoir
{
    public interface ISnapshotFactory
    {
        IRecordedSnapshot<TState> Create<TAggregate, TState>(TAggregate aggregate) where TAggregate : IAggregate<TState>;
    }
}
