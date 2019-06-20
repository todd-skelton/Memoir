using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Memoir.SqlServer
{
    public class EventStore : IEventStore
    {
        private const string _eventColumns = "[EventNumber], [EventId], [EventType], [AggregateId], [AggregateType], [AggregateVersion], [SerializedData], [SerializedMetadata], [Recorded]";
        private readonly ITypeMapper _typeMapper;
        private readonly IEventSerializer _serializer;
        private readonly EventStoreOptions _options;

        public EventStore(ITypeMapper typeMapper, IEventSerializer serializer, EventStoreOptions options)
        {
            _typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public virtual Task Append(Guid aggregateId, long expectedAggregateVersion, IEnumerable<IPendingEvent> pendingEvents)
        {
            // From testing, returning a task has better performance than using await/async on sql methods.
            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_options.ConnectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var @event in pendingEvents)
                        {
                            var command = new SqlCommand($@"INSERT INTO {_options.EventsTableName} VALUES(@eventId, @eventType, @aggregateId, @aggregateType, @aggregateVersion, @serializedData, @serializedMetadata, @recorded)", connection, transaction);
                            command.Parameters.AddWithValue("@eventId", Guid.NewGuid());
                            command.Parameters.AddWithValue("@eventType", @event.EventType);
                            command.Parameters.AddWithValue("@aggregateId", aggregateId);
                            command.Parameters.AddWithValue("@aggregateType", @event.AggregateType);
                            command.Parameters.AddWithValue("@aggregateVersion", ++expectedAggregateVersion);
                            command.Parameters.AddWithValue("@serializedData", _serializer.Serialize(@event.Data));
                            command.Parameters.AddWithValue("@serializedMetadata", _serializer.Serialize(@event.Metadata));
                            command.Parameters.AddWithValue("@recorded", DateTimeOffset.Now);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
            });
        }

        public virtual Task<long> GetLastestEventNumber()
        {
            var query = $"SELECT MAX(EventNumber) FROM {_options.EventsTableName}";

            return Task.Run(() =>
            {
                using (var connection = new SqlConnection(_options.ConnectionString))
                {
                    var command = new SqlCommand(query, connection);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    reader.Read();

                    return reader.GetInt64(0);
                }
            });
        }

        public virtual Task<IEnumerable<IRecordedEvent<TState>>> GetAggregateEvents<TAggregate, TState>(long fromEventNumber, int maxCount) where TAggregate : IAggregate<TState>
        {
            var query = $"SELECT TOP ({maxCount}) {_eventColumns} FROM {_options.EventsTableName} WHERE EventNumber >= {fromEventNumber} AND AggregateType = '{_typeMapper.GetAggregateName<TAggregate, TState>()}'";

            return Task.Run(() => QueryEvents<TState>(query));
        }

        public virtual Task<IEnumerable<IRecordedEvent<TState>>> GetAggregateEvents<TAggregate, TState>(Guid id, long fromAggregateVersion, int maxCount) where TAggregate : IAggregate<TState>
        {
            var query = $"SELECT TOP ({maxCount}) {_eventColumns} FROM {_options.EventsTableName} WHERE AggregateId = '{id}' AND AggregateVersion >= {fromAggregateVersion}";

            return Task.Run(() => QueryEvents<TState>(query));
        }

        public virtual Task<IEnumerable<IRecordedEvent>> GetEvents(long fromEventNumber, int maxCount)
        {
            var query = $"SELECT TOP ({maxCount}) {_eventColumns} FROM {_options.EventsTableName} WHERE EventNumber >= {fromEventNumber}";

            return Task.Run(() => QueryEvents(query));
        }

        public virtual Task<IEnumerable<IRecordedEvent<TEvent, TState>>> GetEvents<TEvent, TState>(long fromEventNumber, int maxCount) where TEvent : IEvent<TState>
        {
            var query = $"SELECT TOP ({maxCount}) {_eventColumns} FROM {_options.EventsTableName} WHERE EventNumber >= {fromEventNumber} AND EventType = '{_typeMapper.GetEventName<TEvent, TState>()}'";

            return Task.Run(() => QueryEvents<TEvent, TState>(query));
        }

        private IEnumerable<IRecordedEvent<TEvent, TState>> QueryEvents<TEvent, TState>(string query) where TEvent : IEvent<TState>
        {
            using (var connection = new SqlConnection(_options.ConnectionString))
            {
                var command = new SqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var eventNumber = reader.GetInt64(0);
                    var eventId = reader.GetGuid(1);
                    var eventType = reader.GetString(2);
                    var aggregateId = reader.GetGuid(3);
                    var aggregateType = reader.GetString(4);
                    var aggregateVersion = reader.GetInt64(5);
                    var serializedData = reader.GetString(6);
                    var serializedMetadata = reader.GetString(7);
                    var recorded = reader.GetDateTimeOffset(8);

                    yield return new RecordedEvent<TEvent, TState>(
                        eventNumber,
                        eventId,
                        eventType,
                        aggregateId,
                        aggregateType,
                        aggregateVersion,
                        _serializer.Deserialize<TEvent>(serializedData),
                        _serializer.Deserialize(serializedMetadata, _typeMapper.GetMetadataType(eventType)) as IMetadata,
                        recorded
                        );
                }
            }
        }

        private IEnumerable<IRecordedEvent<TState>> QueryEvents<TState>(string query)
        {
            using (var connection = new SqlConnection(_options.ConnectionString))
            {
                var command = new SqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var eventNumber = reader.GetInt64(0);
                    var eventId = reader.GetGuid(1);
                    var eventType = reader.GetString(2);
                    var aggregateId = reader.GetGuid(3);
                    var aggregateType = reader.GetString(4);
                    var aggregateVersion = reader.GetInt64(5);
                    var serializedData = reader.GetString(6);
                    var serializedMetadata = reader.GetString(7);
                    var recorded = reader.GetDateTimeOffset(8);

                    yield return new RecordedEvent<TState>(
                        eventNumber,
                        eventId,
                        eventType,
                        aggregateId,
                        aggregateType,
                        aggregateVersion,
                        _serializer.Deserialize(serializedData, _typeMapper.GetEventType(eventType)) as IEvent<TState>,
                        _serializer.Deserialize(serializedMetadata, _typeMapper.GetMetadataType(eventType)) as IMetadata,
                        recorded
                        );
                }
            }
        }

        private IEnumerable<IRecordedEvent> QueryEvents(string query)
        {
            using (var connection = new SqlConnection(_options.ConnectionString))
            {
                var command = new SqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var eventNumber = reader.GetInt64(0);
                    var eventId = reader.GetGuid(1);
                    var eventType = reader.GetString(2);
                    var aggregateId = reader.GetGuid(3);
                    var aggregateType = reader.GetString(4);
                    var aggregateVersion = reader.GetInt64(5);
                    var serializedData = reader.GetString(6);
                    var serializedMetadata = reader.GetString(7);
                    var recorded = reader.GetDateTimeOffset(8);

                    yield return new RecordedEvent(
                        eventNumber,
                        eventId,
                        eventType,
                        aggregateId,
                        aggregateType,
                        aggregateVersion,
                        _serializer.Deserialize(serializedData, _typeMapper.GetEventType(eventType)) as IEvent,
                        _serializer.Deserialize(serializedMetadata, _typeMapper.GetMetadataType(eventType)) as IMetadata,
                        recorded
                        );
                }
            }
        }
    }
}
