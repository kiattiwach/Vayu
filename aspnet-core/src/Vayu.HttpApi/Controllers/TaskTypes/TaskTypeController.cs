using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.Shared;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;

namespace Vayu.Controllers.TaskTypes;

[Route("api/task-type")]
public class TaskTypeController : VayuController, ITaskTypeService
{
    private readonly ITaskTypeService _taskTypeService;

    public TaskTypeController(ITaskTypeService taskTypeService)
    {
        _taskTypeService = taskTypeService;
    }

    [HttpGet("get/{id}")]
    public Task<TaskTypeDto> GetAsync(Guid id)
    {
        return _taskTypeService.GetAsync(id);
    }

    [HttpPost("get-list/{input}")]
    public Task<PagedResultDto<TaskTypeDto>> GetListAsync(TaskTypeFilterDto input)
    {
        return _taskTypeService.GetListAsync(input);
    }

    [HttpPost("create/{input}")]
    public Task<TaskTypeDto> CreateAsync(TaskTypeCreateUpdateDto input)
    {
        return _taskTypeService.CreateAsync(input);
    }

    [HttpPost("update/{input}")]
    public Task<TaskTypeDto> UpdateAsync(TaskTypeCreateUpdateDto input)
    {
        return _taskTypeService.UpdateAsync(input);
    }

    [HttpGet("delete/{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _taskTypeService.DeleteAsync(id);
    }

    [HttpGet("getTaskCategoryList")]
    public Task<PagedResultDto<LookupDto<TaskCategory>>> GetTaskCategoryListAsync(LookupRequestDto input)
    {
        return _taskTypeService.GetTaskCategoryListAsync(input);
    }
}