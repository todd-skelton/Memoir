using System;
using System.Collections.Generic;

namespace Memoir
{
    public abstract class AggregateBase<TState> : IAggregate<TState>
    {
        protected IList<IEvent<TState>> _pendingEvents = new List<IEvent<TState>>();
        private TState _state;

        protected AggregateBase(TState initialState)
        {
            _state = initialState;
        }

        public abstract Guid GetId();
        public long Version { get; private set; }
        public long PendingVersion { get; private set; }

        public virtual IEnumerable<IEvent<TState>> PendingEvents => _pendingEvents;

        public abstract void Issue<TCommand>(TCommand command) where TCommand : ICommand<TState>;

        public virtual void ApplyEvent<TEvent>(TEvent @event) where TEvent : IRecordedEvent<TState>
        {
            _state = On(@event.Data, _state);
            Version++;
            PendingVersion++;
        }

        public virtual void ClearPendingEvents()
        {
            Version = PendingVersion;
            _pendingEvents.Clear();
        }

        protected abstract TState On<TEvent>(TEvent @event, TState state) where TEvent : IEvent<TState>;

        protected virtual void Raise<TEvent>(TEvent @event) where TEvent : IEvent<TState>
        {
            _state = On(@event, _state);
            _pendingEvents.Add(@event);
            PendingVersion++;
        }

        public TState GetState() => _state;

        public void ApplySnapshot<TSnapshot>(TSnapshot snapShot) where TSnapshot : IRecordedSnapshot<TState>
        {
            Version = snapShot.AggregateVersion;
            PendingVersion = snapShot.AggregateVersion;
            _state = snapShot.Data;
        }
    }
}
