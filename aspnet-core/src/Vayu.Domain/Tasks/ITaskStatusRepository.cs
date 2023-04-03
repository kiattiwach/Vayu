using System;
using Volo.Abp.Domain.Repositories;

namespace Vayu.Tasks;

public interface ITaskStatusRepository : IRepository<TaskStatus, Guid>
{
    
}