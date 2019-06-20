using System;

namespace Memoir
{
    public interface ITypeMapper
    {
        Type GetEventType(string name);
        string GetEventName<TEvent, TState>() where TEvent : IEvent<TState>;
        string GetEventName(Type type);
        void MapEvent<T>(string name);
        void MapEvent(string name, Type type);

        Type GetAggregateType(string name);
        string GetAggregateName<TAggregate, TState>() where TAggregate : IAggregate<TState>;
        string GetAggregateName(Type type);
        void MapAggregate<T>(string name);
        void MapAggregate(string name, Type type);

        Type GetMetadataType(string name);
        void MapMetadata<TMetadata>(string name) where TMetadata : IMetadata;
        void MapMetadata(string name, Type type);
    }
}
