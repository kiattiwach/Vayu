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
    private readonly ITaskStaffRepository _taskStaffRepository;
    private readonly ITaskStatusRepository _taskStatusRepository;

    public TaskService(ITaskRepository taskRepository,
        ITaskStaffRepository taskStaffRepository,
        ITaskStatusRepository taskStatusRepository)
    {
        _taskRepository = taskRepository;
        _taskStaffRepository = taskStaffRepository;
        _taskStatusRepository = taskStatusRepository;
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

        await AddTaskStatus(input);


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
        
        var taskStaffs = await _taskStaffRepository.GetListAsync(taskId:input.Id);
        var toDelete = taskStaffs.Where(x => input.TeamMemberIds.All(id => id != x.StaffId));
        await _taskStaffRepository.DeleteManyAsync(toDelete);

        var toInsert = input.TeamMemberIds.Where(id => taskStaffs.All(x => x.StaffId != id))
            .Select(s => new TaskStaff()
            {
                StaffId = s,
                TaskId = input.Id
            });
        await _taskStaffRepository.InsertManyAsync(toInsert);

        await AddTaskStatus(input);

        return ObjectMapper.Map<Task, TaskDto>(result);
    }

    private async System.Threading.Tasks.Task AddTaskStatus(TaskCreateUpdateDto input)
    {
        await _taskStatusRepository.InsertAsync(new TaskStatus()
        {
            TaskId = input.Id,
            Status = input.Status
        });
    }

    [Authorize(VayuPermissions.Tasks.Delete)]
    public System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        return _taskRepository.DeleteAsync(id);
    }
}