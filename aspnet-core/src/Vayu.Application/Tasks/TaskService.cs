using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Vayu.Localization;
using Vayu.Permissions;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Vayu.Tasks;

public class TaskService : VayuAppService, ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskStaffRepository _taskStaffRepository;
    private readonly ITaskStatusRepository _taskStatusRepository;
    private readonly IRepository<TaskType, Guid> _taskTypeRepository;
    private readonly IRepository<Staff, Guid> _staffRepository;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;
    private readonly EmailService _emailService;
    private readonly ICurrentUser _currentUser;
    private readonly MessagingHub _messagingHub;

    public TaskService(ITaskRepository taskRepository,
        ITaskStaffRepository taskStaffRepository,
        ITaskStatusRepository taskStatusRepository,
        IRepository<TaskType, Guid> taskTypeRepository,
        IRepository<Staff, Guid> staffRepository,
        IStringLocalizer<VayuResource> stringLocalize,
        EmailService emailService,
        ICurrentUser currentUser,
        MessagingHub messagingHub)
    {
        _taskRepository = taskRepository;
        _taskStaffRepository = taskStaffRepository;
        _taskStatusRepository = taskStatusRepository;
        _taskTypeRepository = taskTypeRepository;
        _staffRepository = staffRepository;
        _stringLocalize = stringLocalize;
        _emailService = emailService;
        _currentUser = currentUser;
        _messagingHub = messagingHub;
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
        var result = ObjectMapper.Map<List<TaskWithNavigateProperties>, List<TaskWithNavigatePropertiesDto>>(items);
        
        foreach (var taskWithNavigatePropertiesDto in result)
        {
            taskWithNavigatePropertiesDto.TaskStatuses = ObjectMapper.Map<List<TaskStatus>, List<TaskStatusDto>>(
                (await _taskStatusRepository.GetListAsync(ts => ts.TaskId == taskWithNavigatePropertiesDto.Task.Id)));
        }

        return new PagedResultDto<TaskWithNavigatePropertiesDto>(count, result);
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

        await AddTaskStatus(new TaskStatusDto() { TaskId = result.Id, Status = result.Status, Note = null});

        await SendCreateTaskEmail(input);

        await _messagingHub.SendReloadTask(result.Id, _currentUser.Id??Guid.Empty);

        return await ToDto(result);
    }

    private async System.Threading.Tasks.Task SendCreateTaskEmail(TaskCreateUpdateDto input)
    {
        var teamMembers = await _staffRepository.GetListAsync(x => input.TeamMemberIds.Contains(x.Id));
        var leader = await _staffRepository.GetAsync(input.LeaderId);
        var to = new List<Staff>(teamMembers);
        to.Add(leader);
        var taskType = await _taskTypeRepository.GetAsync(input.TaskTypeId);
        var message = _stringLocalize["CreateTaskEmailMessage",
            input.Description,
            input.Note,
            $"{_stringLocalize["ContactInfo"]}-{input.ContactPerson}/{input.ContactNumber}",
            $"{_stringLocalize["Leader"]}-{leader.Name}/{leader.ContactNumber}",
            $"{_stringLocalize["TeamMember"]}-{string.Join(",", teamMembers.Select(x => x.Name))}",
            $"{_stringLocalize["ReceiveDate"]}-{input.CreateDateTime:dd/MM/YY HH:mm}"];

        await _emailService.SendCreateUpdateTaskAsync(to.Select(x => x.Email).ToList(),
            new List<string>() { _currentUser.Email },
            _stringLocalize["CreateTaskEmailSubject", taskType.Name],
            message);
    }

    private async System.Threading.Tasks.Task SendChangeTaskEmail(TaskCreateUpdateDto input, Task oldTask, List<Staff> oldTeamMembers)
    {
        var teamMembers = await _staffRepository.GetListAsync(x => input.TeamMemberIds.Contains(x.Id));
        var leader = await _staffRepository.GetAsync(input.LeaderId);
        var oldLeader = await _staffRepository.GetAsync(oldTask.LeaderId);

        bool hasChangeLeader = false;
        bool hasChangeTeamMember = false;
        bool hasChangeDescription = false;
        bool hasChangeNote = false;
        bool hasChangeContact = false;

        var changeLeader = $"{_stringLocalize["Leader"]}-{leader.Name}/{leader.ContactNumber}";
        hasChangeLeader = oldLeader.Id != input.LeaderId;
        if (hasChangeLeader)
        {
            changeLeader =
                $"{_stringLocalize["Leader"]}-<del>{oldLeader.Name}/{oldLeader.ContactNumber}</del> -> {leader.Name}/{leader.ContactNumber}";
        }

        var changeTeamMember = $"{_stringLocalize["TeamMember"]}-{string.Join(",", teamMembers.Select(x => x.Name))}";
        hasChangeTeamMember = teamMembers.All(t => oldTeamMembers.Any(o => t.Id == o.Id));
        if (hasChangeTeamMember)
        {
            changeTeamMember =
                $"{_stringLocalize["TeamMember"]}-<del>{string.Join(",", oldTeamMembers.Select(x => x.Name))}</del> -> {string.Join(",", teamMembers.Select(x => x.Name))}";
        }

        var changeDescription = input.Description;
        hasChangeDescription = input.Description != oldTask.Description;
        if (hasChangeDescription)
        {
            changeDescription = $"<del>{oldTask.Description}</del> -> {input.Description}";
        }

        var changeNote = input.Note;
        hasChangeNote = input.Note != oldTask.Note;
        if (hasChangeNote)
        {
            changeNote = $"<del>{oldTask.Note}</del> -> {input.Note}";
        }

        var changeContact = $"{_stringLocalize["ContactInfo"]}-{input.ContactPerson}/{input.ContactNumber}";
        hasChangeContact = (input.ContactPerson != oldTask.ContactPerson) || (input.ContactNumber != oldTask.ContactNumber);
        if (hasChangeContact)
        {
            changeContact =
                $"{_stringLocalize["ContactInfo"]}-<del>{oldTask.ContactPerson}/{oldTask.ContactNumber}</del> -> {input.ContactPerson}/{input.ContactNumber}";
        }

        if (!hasChangeDescription && !hasChangeNote && !hasChangeContact && !hasChangeLeader && !hasChangeTeamMember)
        {
            return;
        }

        var to = new List<string>(teamMembers.Select(x => x.Email)) { leader.Email };
        to.AddRange(oldTeamMembers.Select(x => x.Email));
        to = to.Distinct().ToList();

        var message = _stringLocalize["ChangeTaskMessage",
            changeDescription,
            changeNote,
            changeContact,
            changeLeader,
            changeTeamMember,
            $"{_stringLocalize["ReceiveDate"]}-{oldTask.CreateDateTime:dd/MM/YY HH:mm}"];

        await _emailService.SendCreateUpdateTaskAsync(to,
            new List<string>() { _currentUser.Email },
            _stringLocalize["ChangeTaskSubject"],
            message);
    }

    [Authorize(VayuPermissions.Tasks.Edit)]
    public async Task<TaskDto> UpdateAsync(TaskCreateUpdateDto input)
    {
        var task = await _taskRepository.GetAsync(input.Id);
        task.Description = input.Description;
        task.TaskTypeId = input.TaskTypeId;
        task.Note = input.Note;
        task.LeaderId = input.LeaderId;
        task.ContactPerson = input.ContactPerson;
        task.ContactNumber = input.ContactNumber;
        
        var result = await _taskRepository.UpdateAsync(task);

        var oldTeamMembers =
            (await _taskStaffRepository.GetListAsync(input.Id)).Select(x =>
                _staffRepository.GetAsync(x.StaffId).Result);

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

        await SendChangeTaskEmail(input, task, oldTeamMembers.ToList());

        await _messagingHub.SendReloadTask(result.Id, _currentUser.Id ?? Guid.Empty);

        return await ToDto(result);
    }

    private async System.Threading.Tasks.Task AddTaskStatus(TaskStatusDto taskStatus)
    {
        var result = await _taskStatusRepository
            .InsertAsync(new TaskStatus(taskStatus.TaskId, taskStatus.Status, taskStatus.Note));
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

    public async Task<List<StaffDto>> GetAvailableStaffsAsync(Guid[] ids)
    {
        var query = (await _staffRepository.GetQueryableAsync())
            .WhereIf(ids.Any(), x => !ids.Contains(x.Id));
        
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
        result.TaskType = ObjectMapper.Map<TaskType, TaskTypeDto>(await _taskTypeRepository.GetAsync(task.TaskTypeId));

        var taskStatus =
            (await _taskStatusRepository.GetListAsync(x => x.TaskId == task.Id))
            .OrderBy(x => x.CreationTime).ToList();
        
        result.StatusTimeliness = taskStatus
            .Select((x, i) => new TaskStatusTimelineDto()
            {
                Title = GetTitle(x),
                Active = true,
                Label = GetLabel(i),
                Icon = GetIcon(x.Status),
                Color =  x.Status.GetColor()
            }).ToList();

        result.LatestStatusDuration = result.StatusTimeliness?.LastOrDefault()?.Label;
        result.OverAllDuration = GetDuration(result.OverAllTimeSpan);
        result.FinalStatus = result.CompleteDateTime == null ? null : taskStatus?.LastOrDefault()?.Status;

        return result;

        string GetTitle(TaskStatus status)
        {
            return $"{_stringLocalize[$"Enum:Status:{status.Status}"]} {status.Note}";
        }

        string GetLabel(int index)
        {
            TimeSpan timeSpan;
            if (taskStatus.Count == 1 || taskStatus.Count - 1 == index)
            {
                timeSpan = DateTime.Now.Subtract(taskStatus[index].CreationTime);
            }
            else
            {
                timeSpan = taskStatus[index + 1].CreationTime.Subtract(taskStatus[index].CreationTime);
            }

            return GetDuration(timeSpan);
        }

        string GetIcon(Status status)
        {
            var instance = Activator.CreateInstance(typeof(StatusIcon));
            return instance?.GetType()
                .GetProperty(status.ToString())?
                .GetValue(instance)?
                .ToString();
        }
    }

    public async System.Threading.Tasks.Task UpdateTaskStatusAsync(TaskStatusDto input)
    {
        var task = await _taskRepository.GetAsync(input.TaskId);
        var oldStatus = task.Status;

        if (input.Status == Status.Done)
        {
            task.CompleteDateTime = DateTime.Now;
        } 
        else if (task.Status == Status.Done)
        {
            task.CompleteDateTime = null;
        }

        task.Status = input.Status;

        var result = await _taskRepository.UpdateAsync(task);
        await _taskStatusRepository.InsertAsync(new TaskStatus(input.TaskId, input.Status, input.Note));

        await _messagingHub.SendReloadTask(result.Id, _currentUser.Id ?? Guid.Empty);

        await SendChangeStatusEmail(input, task, oldStatus);
    }

    private async System.Threading.Tasks.Task SendChangeStatusEmail(TaskStatusDto input, Task task, Status oldStatus)
    {
        var teamMembers =
            (await _taskStaffRepository.GetListAsync(task.Id)).SelectMany(x =>
                _staffRepository.GetListAsync(s => s.Id == x.StaffId).Result).ToList();
        var leader = await _staffRepository.GetAsync(task.LeaderId);
        var to = new List<Staff>(teamMembers) { leader };
        var message = _stringLocalize["UpdateTaskStatusEmailMessage", _stringLocalize["Enum:Status:" + oldStatus],
            _stringLocalize["Enum:Status:" + input.Status]];

        await _emailService.SendCreateUpdateTaskAsync(to.Select(x => x.Email).ToList(),
            new List<string>() { _currentUser.Email },
            _stringLocalize["UpdateTaskStatusEmailSubject", task.Description],
            message);
    }

    public async Task<List<StaffDto>> GetStaffManyAsync(List<Guid> ids)
    {
        return ObjectMapper.Map<List<Staff>, List<StaffDto>>(await _staffRepository.GetListAsync(x => ids.Contains(x.Id)));
    }

    public Task<PagedResultDto<LookupDto<Status>>> GetTaskStatusListAsync(LookupRequestDto input)
    {
        var taskStatus = Enum.GetValues(typeof(Status))
            .Cast<Status>()
            .WhereIf(input.Filter != null, x => _stringLocalize["Enum:Status:" + x.ToString()].ToString().Contains(input.Filter))
            .Select(s => new LookupDto<Status>()
            {
                Id = s,
                DisplayName = _stringLocalize["Enum:Status:" + s.ToString()]
            }).ToList();

        return System.Threading.Tasks.Task.FromResult(new PagedResultDto<LookupDto<Status>>()
        {
            TotalCount = taskStatus.Count,
            Items = taskStatus
        });
    }

    private string GetDuration(TimeSpan timeSpan)
    {
        return timeSpan.Duration().Days > 0
            ? _stringLocalize["DurationDH", timeSpan.Duration().Days, timeSpan.Duration().Hours, timeSpan.Duration().Hours]
            : _stringLocalize["DurationHM", timeSpan.Duration().Hours, timeSpan.Duration().Minutes];
    }

    public async System.Threading.Tasks.Task TestSignalR()
    {
        await _messagingHub.SendTest(Guid.NewGuid(), Guid.NewGuid());
    }

    [Authorize(VayuPermissions.Tasks.Default)]
    public async Task<TaskWithNavigatePropertiesDto> GetWithNavigateAsync(Guid id)
    {
        var result = ObjectMapper.Map<TaskWithNavigateProperties, TaskWithNavigatePropertiesDto>(await _taskRepository.GetWithNaviagate(id));
        result.TaskStatuses = ObjectMapper.Map<List<TaskStatus>, List<TaskStatusDto>>(
            (await _taskStatusRepository.GetListAsync(ts => ts.TaskId == result.Task.Id)));
        var status = result.Task.Status;
        return result;
    }
}