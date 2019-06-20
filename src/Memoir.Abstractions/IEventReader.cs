using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoir
{
    public interface IEventReader
    {
        Task<long> GetLastestEventNumber();
        Task<IEnumerable<IRecordedEvent>> GetEvents(long fromEventNumber, int maxCount);
        Task<IEnumerable<IRecordedEvent<TEvent, TState>>> GetEvents<TEvent, TState>(long fromEventNumber, int maxCount) where TEvent : IEvent<TState>;
        Task<IEnumerable<IRecordedEvent<TState>>> GetAggregateEvents<TAggregate, TState>(long fromEventNumber, int maxCount) where TAggregate : IAggregate<TState>;
        Task<IEnumerable<IRecordedEvent<TState>>> GetAggregateEvents<TAggregate, TState>(Guid id, long fromAggregateVersion, int maxCount) where TAggregate : IAggregate<TState>;
    }
}
