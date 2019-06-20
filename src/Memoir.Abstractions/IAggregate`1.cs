using System;
using System.Collections.Generic;

namespace Memoir
{
    public interface IAggregate<TState>
    {
        long Version { get; }
        long PendingVersion { get; }
        IEnumerable<IEvent<TState>> PendingEvents { get; }
        Guid GetId();
        TState GetState();
        void Issue<TCommand>(TCommand command) where TCommand : ICommand<TState>;
        void ApplyEvent<TEvent>(TEvent @event) where TEvent : IRecordedEvent<TState>;
        void ApplySnapshot<TSnapshot>(TSnapshot snapShot) where TSnapshot : IRecordedSnapshot<TState>;
        void ClearPendingEvents();
    }
}
