using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;

namespace Vayu.TaskTypes;

public interface ITaskTypeRepository : IBasicRepository<TaskType, Guid>
{
    Task<int> GetCountAsync(string filter = null,
        TaskCategory type = default,
        CancellationToken cancellationToken = default);

    Task<List<TaskType>> GetListAsync(
        string filter = null,
        TaskCategory type = default,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);
}