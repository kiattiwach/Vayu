using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Vayu.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Vayu.Tasks;

public class TaskService : VayuAppService, ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IRepository<TaskStaff> _taskStaffRepository;

    public TaskService(ITaskRepository taskRepository,
        IRepository<TaskStaff> taskStaffRepository)
    {
        _taskRepository = taskRepository;
        _taskStaffRepository = taskStaffRepository;
    }

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<TaskDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Task, TaskDto>(await _taskRepository.GetAsync(id));
    }

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<PagedResultDto<TaskWithNavigatePropertiesDto>> GetListAsync(TaskFilterDto input)
    {
        var count = await _taskRepository.GetCountAsync(input.Filter, input.Name, input.Description, input.TaskTypeId,
            input.Status, input.MinCreateDateTime, input.MaxCreateDateTime, input.MinCompleteDateTime,
            input.MaxCompleteDateTime, input.Note, input.StaffId, input.ContactPerson, input.ContactNumber,
            new List<Guid>() { input.StaffId ?? Guid.Empty });
        var items = await _taskRepository.GetListAsync(input.Filter, input.Name, input.Description, input.TaskTypeId,
            input.Status, input.MinCreateDateTime, input.MaxCreateDateTime, input.MinCompleteDateTime,
            input.MaxCompleteDateTime, input.Note, input.StaffId, input.ContactPerson, input.ContactNumber,
            new List<Guid>() { input.StaffId ?? Guid.Empty }, input.MaxResultCount, input.SkipCount, input.Sorting);

        return new PagedResultDto<TaskWithNavigatePropertiesDto>(count,
            ObjectMapper.Map<List<TaskWithNavigateProperties>, List<TaskWithNavigatePropertiesDto>>(items));
    }

    [Authorize(VayuPermissions.Tasks.Create)]
    public async Task<TaskDto> CreateAsync(TaskCreateUpdateDto input)
    {
        var result = await _taskRepository.InsertAsync(new Task(input.Name, input.Description, input.TaskTypeId, input.Status,
            input.CreateDateTime, input.CompleteDateTime, input.Note, input.LeaderId, input.ContactPerson,input.ContactNumber));

        await _taskStaffRepository.InsertManyAsync(input.TeamMemberIds.Select(s => new TaskStaff()
        {
            StaffId = s,
            TaskId = result.Id
        }));

        return ObjectMapper.Map<Task, TaskDto>(result);
    }

    [Authorize(VayuPermissions.Tasks.Edit)]
    public async Task<TaskDto> UpdateAsync(TaskCreateUpdateDto input)
    {
        var task = await _taskRepository.GetAsync(input.Id);
        task.Name = input.Name;
        task.Description = input.Description;
        task.TaskTypeId = input.TaskTypeId;
        task.Status = input.Status;
        task.CreateDateTime = input.CreateDateTime;
        task.CompleteDateTime = input.CompleteDateTime;
        task.Note = input.Note;
        task.LeaderId = input.LeaderId;
        task.ContactPerson = input.ContactPerson;
        task.ContactNumber = input.ContactNumber;
        
        var result = await _taskRepository.UpdateAsync(task);

        var taskStaffs = (await _taskStaffRepository.GetQueryableAsync()).Where(x => x.TaskId == input.Id);
        await _taskStaffRepository.DeleteAsync(x => input.TeamMemberIds.All(t => x.StaffId != t));
        await _taskStaffRepository.InsertManyAsync(input.TeamMemberIds.Where(t => !taskStaffs.Any(x => t == x.StaffId))
            .Select(s => new TaskStaff()
            {
                StaffId = s,
                TaskId = input.Id
            }));

        return ObjectMapper.Map<Task, TaskDto>(result);
    }

    [Authorize(VayuPermissions.Tasks.Delete)]
    public System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        return _taskRepository.DeleteAsync(id);
    }
}