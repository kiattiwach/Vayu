using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vayu.EntityFrameworkCore;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Tasks;

public class TaskRepository : EfCoreRepository<VayuDbContext, Task, Guid>, ITaskRepository
{
    public TaskRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
    
    public async Task<int> GetCountAsync(string filter = null, string description = null,
        Guid? taskTypeId = null,
        Status? status = null, DateTime? minCreateDateTime = null,
        DateTime? maxCreateDateTime = null, DateTime? minCompleteDateTime = null, DateTime? maxCompleteDateTime = null,
        string note = null, Guid? leaderId = null, string contactPerson = null, string contactNumber = null,
        List<Guid> teamMemberIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQuery(filter, description, taskTypeId, status, minCreateDateTime, maxCreateDateTime,
            minCompleteDateTime, maxCompleteDateTime, note, leaderId, contactPerson, contactNumber, teamMemberIds);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<TaskWithNavigateProperties>> GetListAsync(string filter = null, string description = null,
        Guid? taskTypeId = null,
        Status? status = null, DateTime? minCreateDateTime = null,
        DateTime? maxCreateDateTime = null, DateTime? minCompleteDateTime = null, DateTime? maxCompleteDateTime = null,
        string note = null, Guid? leaderId = null, string contactPerson = null, string contactNumber = null,
        List<Guid> teamMemberIds = null, int maxResultCount = 2147483647, int skipCount = 0, string sorting = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQuery(filter, description, taskTypeId, status, minCreateDateTime, maxCreateDateTime,
            minCompleteDateTime, maxCompleteDateTime, note, leaderId, contactPerson, contactNumber, teamMemberIds);
        query = query.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Task)}.{nameof(Task.CreateDateTime)} desc" : sorting);
        return await query.Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    private async Task<IQueryable<TaskWithNavigateProperties>> GetQuery(string filter = null, string description = null, Guid? taskTypeId = null,
        Status? status = null, DateTime? minCreateDateTime = null,
        DateTime? maxCreateDateTime = null, DateTime? minCompleteDateTime = null, DateTime? maxCompleteDateTime = null,
        string note = null, Guid? leaderId = null, string contactPerson = null, string contactNumber = null,
        List<Guid> teamMemberIds = null)
    {
        var taskQuery = (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Description.Contains(filter)
                || x.Note.Contains(filter) || x.ContactPerson.Contains(filter) || x.ContactNumber.Contains(filter))
            .WhereIf(!description.IsNullOrWhiteSpace(), x => x.Description.Contains(description))
            .WhereIf(taskTypeId != Guid.Empty && taskTypeId != null, x => x.TaskTypeId == taskTypeId)
            .WhereIf(status != null, x => x.Status == status)
            .WhereIf(minCreateDateTime != null, x => x.CreateDateTime > minCreateDateTime)
            .WhereIf(maxCreateDateTime != null, x => x.CreateDateTime < maxCreateDateTime)
            .WhereIf(minCompleteDateTime != null, x => x.CompleteDateTime > minCompleteDateTime)
            .WhereIf(maxCompleteDateTime != null, x => x.CreateDateTime < maxCompleteDateTime)
            .WhereIf(!note.IsNullOrWhiteSpace(), x => x.Note.Contains(note))
            .WhereIf(leaderId != Guid.Empty && leaderId != null, x => x.LeaderId == leaderId)
            .WhereIf(!contactPerson.IsNullOrWhiteSpace(), x => x.ContactPerson.Contains(contactPerson))
            .WhereIf(!contactNumber.IsNullOrWhiteSpace(), x => x.ContactNumber.Contains(contactNumber));

        var taskStaffs = from taskStaff in (await GetDbContextAsync()).Set<TaskStaff>()
            join staff in (await GetDbContextAsync()).Set<Staff>() on taskStaff.StaffId equals staff.Id
            join task in taskQuery on taskStaff.TaskId equals task.Id
            where teamMemberIds.Contains(taskStaff.StaffId)
                select staff;

        var result = from task in taskQuery
            join taskType in (await GetDbContextAsync()).Set<TaskType>() on task.TaskTypeId equals taskType.Id
            join leader in (await GetDbContextAsync()).Set<Staff>() on task.LeaderId equals leader.Id

            select new TaskWithNavigateProperties()
            {
                Task = task,
                TaskType = taskType,
                Reader = leader,
                TeamMembers = taskStaffs.ToList()
            };

        return result;
    }
}