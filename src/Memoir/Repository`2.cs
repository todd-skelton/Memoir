using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoir
{
    public class Repository<TAggregate, TState> : IRepository<TAggregate, TState> where TAggregate : IAggregate<TState>
    {
        private readonly IEventStore _eventStore;
        private readonly IPendingEventFactory _pendingEventFactory;
        private readonly ISnapshotStore _snapShotStore;
        private readonly ISnapshotFactory _snapshotFactory;

        public Repository(IEventStore eventStore, IPendingEventFactory pendingEventFactory, ISnapshotStore snapShotStore, ISnapshotFactory snapshotFactory)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _pendingEventFactory = pendingEventFactory ?? throw new ArgumentNullException(nameof(pendingEventFactory));
            _snapShotStore = snapShotStore ?? throw new ArgumentNullException(nameof(snapShotStore));
            _snapshotFactory = snapshotFactory ?? throw new ArgumentNullException(nameof(snapshotFactory));
        }

        public async Task Load(TAggregate aggregate, long? toVersion = null)
        {
            int maxCount = toVersion.HasValue ? (int)(toVersion.Value - aggregate.Version) : int.MaxValue;

            var snapshot = await _snapShotStore.GetSnapshot<TState>(aggregate.GetId());

            if(snapshot != null && snapshot.AggregateVersion < toVersion)
            {
                aggregate.ApplySnapshot(snapshot);
            }

            var events = await _eventStore.GetAggregateEvents<TAggregate, TState>(aggregate.GetId(), aggregate.Version + 1, maxCount);

            foreach (var @event in events)
            {
                aggregate.ApplyEvent(@event);
            }
        }

        public async Task Save(TAggregate aggregate, bool takeSnapshot = false)
        {
            await _eventStore.Append(aggregate.GetId(), aggregate.Version, GetPendingEvents(aggregate));
            aggregate.ClearPendingEvents();

            if (takeSnapshot)
            {
                var snapshot = _snapshotFactory.Create<TAggregate, TState>(aggregate);

                await _snapShotStore.TakeSnapshot(snapshot);
            }
        }

        private IEnumerable<IPendingEvent> GetPendingEvents(TAggregate aggregate)
        {
            foreach (var @event in aggregate.PendingEvents)
            {
                yield return _pendingEventFactory.Create(aggregate, @event);
            }
        }
    }
}
