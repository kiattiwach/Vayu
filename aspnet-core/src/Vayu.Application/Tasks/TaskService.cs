using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Vayu.Permissions;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Vayu.Tasks;

public class TaskService : VayuAppService, ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskStaffRepository _taskStaffRepository;
    private readonly ITaskStatusRepository _taskStatusRepository;
    private readonly IRepository<TaskType, Guid> _taskTypeRepository;
    private readonly IRepository<Staff, Guid> _staffRepository;

    public TaskService(ITaskRepository taskRepository,
        ITaskStaffRepository taskStaffRepository,
        ITaskStatusRepository taskStatusRepository,
        IRepository<TaskType, Guid> taskTypeRepository,
        IRepository<Staff, Guid> staffRepository)
    {
        _taskRepository = taskRepository;
        _taskStaffRepository = taskStaffRepository;
        _taskStatusRepository = taskStatusRepository;
        _taskTypeRepository = taskTypeRepository;
        _staffRepository = staffRepository;
    }

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<TaskDto> GetAsync(Guid id)
    {
        var task = await _taskRepository.GetAsync(id);
        var taskDto = await ToDto(task);

        return taskDto;
    }

    private async Task<TaskDto> ToDto(Task task)
    {
        var taskDto = ObjectMapper.Map<Task, TaskDto>(task);
        var leader = await _staffRepository.GetAsync(task.LeaderId);
        var teamMember = (await _taskStaffRepository.GetListAsync(task.Id))?
            .Select(x => _staffRepository.GetAsync(x.StaffId).Result).ToList();

        taskDto.Leader = ObjectMapper.Map<Staff, StaffDto>(leader);
        taskDto.TeamMembers = ObjectMapper.Map<List<Staff>, List<StaffDto>>(teamMember);
        return taskDto;
    }

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<PagedResultDto<TaskWithNavigatePropertiesDto>> GetListAsync(TaskFilterDto input)
    {
        var count = await _taskRepository.GetCountAsync(input.Filter, input.Description, input.TaskTypeId,
            input.Status, input.MinCreateDateTime, input.MaxCreateDateTime, input.MinCompleteDateTime,
            input.MaxCompleteDateTime, input.Note, input.StaffId, input.ContactPerson, input.ContactNumber,
            new List<Guid>() { input.StaffId ?? Guid.Empty });
        var items = await _taskRepository.GetListAsync(input.Filter, input.Description, input.TaskTypeId,
            input.Status, input.MinCreateDateTime, input.MaxCreateDateTime, input.MinCompleteDateTime,
            input.MaxCompleteDateTime, input.Note, input.StaffId, input.ContactPerson, input.ContactNumber,
            new List<Guid>() { input.StaffId ?? Guid.Empty }, input.MaxResultCount, input.SkipCount, input.Sorting);

        return new PagedResultDto<TaskWithNavigatePropertiesDto>(count,
            ObjectMapper.Map<List<TaskWithNavigateProperties>, List<TaskWithNavigatePropertiesDto>>(items));
    }

    [Authorize(VayuPermissions.Tasks.Create)]
    public async Task<TaskDto> CreateAsync(TaskCreateUpdateDto input)
    {
        var result = await _taskRepository.InsertAsync(new Task(input.Description, input.TaskTypeId, input.Status,
            input.CreateDateTime, input.CompleteDateTime, input.Note, input.Leader.Id, input.ContactPerson,input.ContactNumber));

        await _taskStaffRepository.InsertManyAsync(input.TeamMemberIds.Select(s => new TaskStaff()
        {
            StaffId = s.Id,
            TaskId = result.Id
        }));

        await AddTaskStatus(input);


        return await ToDto(result);
    }

    [Authorize(VayuPermissions.Tasks.Edit)]
    public async Task<TaskDto> UpdateAsync(TaskCreateUpdateDto input)
    {
        var task = await _taskRepository.GetAsync(input.Id);
        task.Description = input.Description;
        task.TaskTypeId = input.TaskTypeId;
        task.Status = input.Status;
        task.CreateDateTime = input.CreateDateTime;
        task.CompleteDateTime = input.CompleteDateTime;
        task.Note = input.Note;
        task.LeaderId = input.Leader.Id;
        task.ContactPerson = input.ContactPerson;
        task.ContactNumber = input.ContactNumber;
        
        var result = await _taskRepository.UpdateAsync(task);
        
        var taskStaffs = await _taskStaffRepository.GetListAsync(taskId:input.Id);
        var toDelete = taskStaffs.Where(x => input.TeamMemberIds.All(staff => staff.Id != x.StaffId));
        await _taskStaffRepository.DeleteManyAsync(toDelete);

        var toInsert = input.TeamMemberIds.Where(staff => taskStaffs.All(x => x.StaffId != staff.Id))
            .Select(s => new TaskStaff()
            {
                StaffId = s.Id,
                TaskId = input.Id
            });
        await _taskStaffRepository.InsertManyAsync(toInsert);

        await AddTaskStatus(input);

        return await ToDto(result);
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

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<PagedResultDto<LookupDto<Guid>>> GetTaskTypeLookupAsync(LookupRequestDto input)
    {
        var query = (await _taskTypeRepository.GetQueryableAsync())
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter));
        var items = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<TaskType>();
        return new PagedResultDto<LookupDto<Guid>>(items.Count, ObjectMapper.Map<List<TaskType>, List<LookupDto<Guid>>>(items));
    }

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<PagedResultDto<LookupDto<Guid>>> GetStaffLookupAsync(LookupRequestDto input)
    {
        var query = (await _staffRepository.GetQueryableAsync())
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter));
        var items = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Staff>();
        return new PagedResultDto<LookupDto<Guid>>(items.Count, ObjectMapper.Map<List<Staff>, List<LookupDto<Guid>>>(items));
    }
}