using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Vayu.Tasks;

public interface ITaskRepository : IBasicRepository<Task, Guid>
{
    Task<int> GetCountAsync(string filter = null,
        string name = null,
        string description = null,
        Guid? taskTypeId = null,
        Status? status = null,
        DateTime? minCreateDateTime = null,
        DateTime? maxCreateDateTime = null,
        DateTime? minCompleteDateTime = null,
        DateTime? maxCompleteDateTime = null,
        string note = null,
        Guid? leaderId = null,
        string contactPerson = null,
        string contactNumber = null,
        List<Guid> teamMemberIds = null,
        CancellationToken cancellationToken = default);

    Task<List<TaskWithNavigateProperties>> GetListAsync(string filter = null,
        string name = null,
        string description = null,
        Guid? taskTypeId = null,
        Status? status = null,
        DateTime? minCreateDateTime = null,
        DateTime? maxCreateDateTime = null,
        DateTime? minCompleteDateTime = null,
        DateTime? maxCompleteDateTime = null,
        string note = null,
        Guid? leaderId = null,
        string contactPerson = null,
        string contactNumber = null,
        List<Guid> teamMemberIds = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);
}