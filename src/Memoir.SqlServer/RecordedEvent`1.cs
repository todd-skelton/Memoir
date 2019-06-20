using System;
using System.Collections.Generic;
using System.Text;

namespace Memoir.SqlServer
{
    public class RecordedEvent<TState> : IRecordedEvent<TState>
    {
        public RecordedEvent(long eventNumber, Guid eventId, string eventType, Guid aggregateId, string aggregateType, long aggregateVersion, IEvent<TState> data, IMetadata metaData, DateTimeOffset recorded)
        {
            EventNumber = eventNumber;
            EventId = eventId;
            EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
            AggregateId = aggregateId;
            AggregateType = aggregateType ?? throw new ArgumentNullException(nameof(aggregateType));
            AggregateVersion = aggregateVersion;
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Metadata = metaData ?? throw new ArgumentNullException(nameof(metaData));
            Recorded = recorded;
        }

        public long EventNumber { get; private set; }

        public Guid EventId { get; private set; }

        public string EventType { get; private set; }

        public Guid AggregateId { get; private set; }

        public string AggregateType { get; private set; }

        public long AggregateVersion { get; private set; }

        public IEvent<TState> Data { get; private set; }

        public IMetadata Metadata { get; private set; }

        public DateTimeOffset Recorded { get; private set; }

        IEvent IRecordedEvent.Data => Data;
    }
}
