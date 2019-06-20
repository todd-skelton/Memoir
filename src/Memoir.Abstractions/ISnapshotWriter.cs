using System.Threading.Tasks;

namespace Memoir
{
    public interface ISnapshotWriter
    {
        Task TakeSnapshot<TState>(IRecordedSnapshot<TState> snapshot);
    }
}
