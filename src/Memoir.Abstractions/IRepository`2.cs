using System.Threading.Tasks;

namespace Memoir
{
    public interface IRepository<TAggregate, TState> where TAggregate : IAggregate<TState>
    {
        /// <summary>
        /// Loads the aggregate with the events from the event store.
        /// </summary>
        /// <param name="aggregate">The aggregate to load with its id set.</param>
        /// <param name="toVersion">Aggregate version to end with.</param>
        Task Load(TAggregate aggregate, long? toVersion = null);
        Task Save(TAggregate aggregate, bool takeSnapshot = false);
    }
}
