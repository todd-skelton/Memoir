using System;

namespace Memoir
{
    public interface IRecordedSnapshot<TState>
    {
        Guid AggregateId { get; }
        string AggregateType { get; }
        long AggregateVersion { get; }
        TState Data { get; }
        IMetadata Metadata { get; }
        DateTimeOffset Recorded { get; }
    }
}
