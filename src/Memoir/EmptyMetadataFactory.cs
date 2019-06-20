namespace Memoir
{
    public class EmptyMetadataFactory : IMetadataFactory
    {
        public IMetadata Create<TAggregate, TState>(TAggregate aggregate) where TAggregate : IAggregate<TState> => new EmptyMetadata();
    }
}
