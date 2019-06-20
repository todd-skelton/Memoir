using System.Threading.Tasks;

namespace Memoir
{
    public interface IProjector<TEvent, TProjection> where TEvent : IEvent
    {
        Task<TProjection> Project(IRecordedEvent<TEvent> recordedEvent);
    }
}
