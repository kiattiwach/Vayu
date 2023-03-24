using System;
using Vayu.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Tasks;

public class TaskStatusRepository : EfCoreRepository<VayuDbContext, TaskStatus, Guid>, ITaskStatusRepository
{
    public TaskStatusRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}