using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vayu.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Tasks;

public class TaskStaffRepository : EfCoreRepository<VayuDbContext, TaskStaff>, ITaskStaffRepository
{
    public TaskStaffRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<TaskStaff>> GetListAsync(Guid? taskId = null, Guid? staffId = null)
    {
        return(await GetDbSetAsync())
            .WhereIf(taskId != Guid.Empty && taskId != null, x => x.TaskId == taskId)
            .WhereIf(staffId != Guid.Empty && staffId != null, x => x.StaffId == staffId)
            .ToList();
    }
}