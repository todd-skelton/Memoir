namespace Memoir
{
    public interface IMetadataFactory
    {
        IMetadata Create<TAggregate, TState>(TAggregate aggregate) where TAggregate : IAggregate<TState>;
    }
}
