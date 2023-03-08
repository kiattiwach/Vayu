using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Vayu.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.TaskTypes;

public class TaskTypeRepository : EfCoreRepository<VayuDbContext, TaskType, Guid>, ITaskTypeRepository
{
    public TaskTypeRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<int> GetCountAsync(string filter = null, TaskTypeEnum type = default,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQuery(filter, type);
        return await queryable.CountAsync(GetCancellationToken(cancellationToken)); ;
    }

    public async Task<List<TaskType>> GetListAsync(string filter = null, TaskTypeEnum type = default, int maxResultCount = Int32.MaxValue,
        int skipCount = 0, string sorting = null, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQuery(filter, type);
        queryable = queryable.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(TaskType.Name)} desc" : sorting);
        return await queryable.Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    private async Task<IQueryable<TaskType>> GetQuery(string filter = null, TaskTypeEnum type = default)
    {
        return (await GetDbSetAsync())
            .WhereIf(type != default, x => x.Type == type)
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter));
    }
}