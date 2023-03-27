using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.Shared;
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
}