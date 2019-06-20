using System;
using System.Threading.Tasks;

namespace Memoir
{
    public interface ISnapshotReader
    {
        Task<IRecordedSnapshot<TState>> GetSnapshot<TState>(Guid aggregateId);
    }
}
