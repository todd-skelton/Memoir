using System;

namespace Memoir
{
    public class SnapshotFactory : ISnapshotFactory
    {
        private readonly ITypeMapper _typeMapper;
        private readonly IMetadataFactory _metadataFactory;

        public SnapshotFactory(ITypeMapper typeMapper, IMetadataFactory metadataFactory)
        {
            _typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
            _metadataFactory = metadataFactory ?? throw new ArgumentNullException(nameof(metadataFactory));
        }

        public IRecordedSnapshot<TState> Create<TAggregate, TState>(TAggregate aggregate)
            where TAggregate : IAggregate<TState>
        {
            return new RecordedSnapshot<TState>(aggregate.GetId(), _typeMapper.GetAggregateName<TAggregate, TState>(), aggregate.Version, aggregate.GetState(), _metadataFactory.Create<TAggregate, TState>(aggregate), DateTimeOffset.Now);
        }
    }
}
