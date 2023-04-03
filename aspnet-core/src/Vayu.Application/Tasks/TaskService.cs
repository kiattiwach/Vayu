using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Vayu.Localization;
using Vayu.Permissions;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Vayu.Tasks;

public class TaskService : VayuAppService, ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskStaffRepository _taskStaffRepository;
    private readonly ITaskStatusRepository _taskStatusRepository;
    private readonly IRepository<TaskType, Guid> _taskTypeRepository;
    private readonly IRepository<Staff, Guid> _staffRepository;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;

    public TaskService(ITaskRepository taskRepository,
        ITaskStaffRepository taskStaffRepository,
        ITaskStatusRepository taskStatusRepository,
        IRepository<TaskType, Guid> taskTypeRepository,
        IRepository<Staff, Guid> staffRepository,
        IStringLocalizer<VayuResource> stringLocalize)
    {
        _taskRepository = taskRepository;
        _taskStaffRepository = taskStaffRepository;
        _taskStatusRepository = taskStatusRepository;
        _taskTypeRepository = taskTypeRepository;
        _staffRepository = staffRepository;
        _stringLocalize = stringLocalize;
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
        var teamMemberIds = (await _taskStaffRepository.GetListAsync(task.Id))?
            .Select(x => _staffRepository.GetAsync(x.StaffId).Result.Id).ToList();

        taskDto.TeamMemberIds = teamMemberIds;
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
        input.CreateDateTime = DateTime.Now;
        input.Status = Status.Initial;

        var result = await _taskRepository.InsertAsync(new Task(input.Description, input.TaskTypeId, input.Status,
            input.CreateDateTime, null, input.Note, input.LeaderId, input.ContactPerson,input.ContactNumber));

        await _taskStaffRepository.InsertManyAsync(input.TeamMemberIds.Select(s => new TaskStaff()
        {
            StaffId = s,
            TaskId = result.Id
        }));

        await AddTaskStatus(result);

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

        await AddTaskStatus(result);

        return await ToDto(result);
    }

    private async System.Threading.Tasks.Task AddTaskStatus(Task task)
    {
        var result = await _taskStatusRepository.InsertAsync(new TaskStatus()
        {
            TaskId = task.Id,
            Status = task.Status
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

    public async Task<List<StaffDto>> GetTeamLookupAsync(List<Guid> ids)
    {
        var query = (await _staffRepository.GetQueryableAsync())
            .WhereIf(ids.Any(), x => ids.Contains(x.Id));
        
        var items = query.ToList();
        return ObjectMapper.Map<List<Staff>, List<StaffDto>>(items);
    }

    public async Task<TaskViewDto> GetTaskInfoAsync(Guid id)
    {
        var task = await _taskRepository.GetAsync(id);
        if (task == null)
        {
            return null;
        }

        var taskStaffIds = (await _taskStaffRepository.GetListAsync(taskId: task.Id))?.Select(s => s.StaffId);
        var result = ObjectMapper.Map<Task,TaskViewDto>(task);
        result.Leader = ObjectMapper.Map<Staff, StaffDto>(await _staffRepository.GetAsync(task.LeaderId));
        result.TeamMembers =
            ObjectMapper.Map<List<Staff>, List<StaffDto>>(
                await _staffRepository.GetListAsync(x => taskStaffIds.Contains(x.Id)));


        var taskStatus =
            (await _taskStatusRepository.GetListAsync(x => x.TaskId == task.Id))
            .OrderBy(x => x.CreationTime).ToList();
        
        result.StatusTimeliness = taskStatus
            .Select((x, i) => new TaskStatusTimelineDto()
            {
                Title = x.Status.ToString(),
                Active = i == taskStatus.Count - 1,
                Label = GetLabel(i),
                Icon = GetIcon(x.Status),
                Color = GetColor(x.Status)
            }).ToList();

        return result;

        string GetLabel(int index)
        {
            TimeSpan timeSpan;
            if (taskStatus.Count == 1)
            {
                timeSpan = DateTime.Now.Subtract(taskStatus[index].CreationTime);
            } else if (taskStatus.Count - 1 == index)
            {
                return string.Empty;
            }
            else
            {
                timeSpan = taskStatus[index + 1].CreationTime.Subtract(taskStatus[index].CreationTime);
            }

            if (timeSpan.Duration().Days > 0)
            {
                return _stringLocalize["DurationDHM", timeSpan.Duration().Days, timeSpan.Duration().Hours,
                    timeSpan.Duration().Minutes];
            }
            else
            {
                return _stringLocalize["DurationHM", timeSpan.Duration().Hours, timeSpan.Duration().Minutes];
            }
        }

        string GetIcon(Status status)
        {
            var instance = Activator.CreateInstance(typeof(StatusIcon));

            return instance?.GetType()
                .GetProperty(status.ToString())?
                .GetValue(instance)?
                .ToString();
        }

        string GetColor(Status status)
        {
            var instance = Activator.CreateInstance(typeof(StatusColor));

            var color = (Color)instance?.GetType()?
                .GetProperty(status.ToString())?
                .GetValue(instance);
            
            return ColorTranslator.ToHtml(color);
        }
    }
}