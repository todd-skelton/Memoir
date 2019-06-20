using System;

namespace Memoir
{
    internal class RecordedSnapshot<TState> : IRecordedSnapshot<TState>
    {
        public RecordedSnapshot(Guid aggregateId, string aggregateType, long aggregateVersion, TState data, IMetadata metadata, DateTimeOffset recorded)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            AggregateVersion = aggregateVersion;
            Data = data;
            Metadata = metadata;
            Recorded = recorded;
        }

        public Guid AggregateId { get; }

        public string AggregateType { get; }

        public long AggregateVersion { get; }

        public TState Data { get; }

        public IMetadata Metadata { get; }

        public DateTimeOffset Recorded { get; }
    }
}
