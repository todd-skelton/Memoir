using System;

namespace Memoir
{
    public class PendingEventFactory : IPendingEventFactory
    {
        private readonly ITypeMapper _typeMapper;
        private readonly IMetadataFactory _metadataFactory;

        public PendingEventFactory(ITypeMapper typeMapper, IMetadataFactory metadataFactory)
        {
            _typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
            _metadataFactory = metadataFactory ?? throw new ArgumentNullException(nameof(metadataFactory));
        }

        public IPendingEvent Create<TAggregate, TState>(TAggregate aggregate, IEvent<TState> @event) where TAggregate : IAggregate<TState>
        {
            return new PendingEvent(Guid.NewGuid(), _typeMapper.GetEventName(@event.GetType()), aggregate.GetId(), _typeMapper.GetAggregateName<TAggregate, TState>(), @event, _metadataFactory.Create<TAggregate, TState>(aggregate));
        }
    }
}
