using System;
using System.Threading;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }

}
