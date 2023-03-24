using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Vayu.Tasks;

public interface ITaskStaffRepository : IBasicRepository<TaskStaff>
{
    Task<List<TaskStaff>> GetListAsync(Guid? taskId = null, Guid? staffId = null);
}