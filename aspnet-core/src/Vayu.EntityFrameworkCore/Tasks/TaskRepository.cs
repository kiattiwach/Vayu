using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vayu.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Tasks;

public class TaskRepository : EfCoreRepository<VayuDbContext, Task, Guid>, ITaskRepository
{
    public TaskRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<int> GetCountAsync(string filter = null, string name = null, string description = null, Guid? taskTypeId = null,
        Status? status = null, string address = null, DateTime? createDateTime = null, DateTime? completeDateTime = null,
        string note = null, Guid? loaderId = null, string contactPerson = null, string contactNumber = null,
        List<Guid> teamMemberIds = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Task>> GetListAsync(string filter = null, string name = null, string description = null, Guid? taskTypeId = null,
        Status? status = null, string address = null, DateTime? createDateTime = null, DateTime? completeDateTime = null,
        string note = null, Guid? loaderId = null, string contactPerson = null, string contactNumber = null,
        List<Guid> teamMemberIds = null, int maxResultCount = Int32.MaxValue, int skipCount = 0, string sorting = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<IQueryable<Task>> GetQuery(string filter = null, string name = null, string description = null, Guid? taskTypeId = null,
        Status? status = null, string address = null, DateTime? createDateTime = null, DateTime? completeDateTime = null,
        string note = null, Guid? loaderId = null, string contactPerson = null, string contactNumber = null,
        List<Guid> teamMemberIds = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(),x => x.Name.Contains(filter) )
    }
}