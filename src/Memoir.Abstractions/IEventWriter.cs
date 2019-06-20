using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoir
{
    public interface IEventWriter
    {
        Task Append(Guid aggregateId, long expectedAggregateVersion, IEnumerable<IPendingEvent> pendingEvents);
    }
}
