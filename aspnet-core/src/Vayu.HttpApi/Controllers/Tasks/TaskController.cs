using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.Tasks;
using Volo.Abp.Application.Dtos;

namespace Vayu.Controllers.Tasks;

[Route("api/task")]
public class TaskController : VayuController, ITaskService
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("get/{id}")]
    public Task<TaskDto> GetAsync(Guid id)
    {
        return _taskService.GetAsync(id);
    }

    [HttpPost("get-list{input}")]
    public Task<PagedResultDto<TaskWithNavigatePropertiesDto>> GetListAsync(TaskFilterDto input)
    {
        return _taskService.GetListAsync(input);
    }

    [HttpPost("create/{input}")]
    public Task<TaskDto> CreateAsync(TaskCreateUpdateDto input)
    {
        return _taskService.CreateAsync(input);
    }

    [HttpPost("update/{input}")]
    public Task<TaskDto> UpdateAsync(TaskCreateUpdateDto input)
    {
        return _taskService.UpdateAsync(input);
    }

    [HttpGet("delete/{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _taskService.DeleteAsync(id);
    }

    [HttpPost("get-task-type-lookup/{input}")]
    public Task<PagedResultDto<LookupDto<Guid>>> GetTaskTypeLookupAsync(LookupRequestDto input)
    {
        return _taskService.GetTaskTypeLookupAsync(input);
    }

    [HttpPost("get-staff-lookup/{input}")]
    public Task<PagedResultDto<LookupDto<Guid>>> GetStaffLookupAsync(LookupRequestDto input)
    {
        return _taskService.GetStaffLookupAsync(input);
    }

    [HttpPost("get-team-lookup/{ids}")]
    public Task<List<StaffDto>> GetTeamLookupAsync(List<Guid> ids)
    {
        return _taskService.GetTeamLookupAsync(ids);
    }

    [HttpGet("get-task-info")]
    public Task<TaskViewDto> GetTaskInfoAsync(Guid id)
    {
        return _taskService.GetTaskInfoAsync(id);
    }

    [HttpPost("update-task-status/{input}")]
    public Task UpdateTaskStatusAsync(TaskStatusDto input)
    {
        return _taskService.UpdateTaskStatusAsync(input);
    }

    [HttpPost("get-staff-many/{ids}")]
    public Task<List<StaffDto>> GetStaffManyAsync(List<Guid> ids)
    {
        return _taskService.GetStaffManyAsync(ids);
    }
}