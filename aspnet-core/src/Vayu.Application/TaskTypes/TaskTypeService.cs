using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Vayu.Localization;
using Vayu.Permissions;
using Vayu.Shared;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Vayu.TaskTypes;

public class TaskTypeService : VayuAppService, ITaskTypeService
{
    private readonly ITaskTypeRepository _taskTypeRepository;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;

    public TaskTypeService(ITaskTypeRepository taskTypeRepository,IStringLocalizer<VayuResource> stringLocalize)
    {
        _taskTypeRepository = taskTypeRepository;
        _stringLocalize = stringLocalize;
    }

    [Authorize(VayuPermissions.TaskTypes.Default)]
    public async Task<TaskTypeDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<TaskType, TaskTypeDto>(await _taskTypeRepository.GetAsync(id));
    }

    [Authorize(VayuPermissions.TaskTypes.Default)]
    public async Task<PagedResultDto<TaskTypeDto>> GetListAsync(TaskTypeFilterDto input)
    {
        var count = await _taskTypeRepository.GetCountAsync(input.Filter, input.TaskCategory);
        var items = await _taskTypeRepository.GetListAsync(input.Filter, input.TaskCategory,
            input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<TaskTypeDto>
            { TotalCount = count, Items = ObjectMapper.Map<List<TaskType>, List<TaskTypeDto>>(items) };
    }

    [Authorize(VayuPermissions.TaskTypes.Create)]
    public async Task<TaskTypeDto> CreateAsync(TaskTypeCreateUpdateDto input)
    {
        var result= await _taskTypeRepository.InsertAsync(new TaskType(input.Name, input.TaskCategory));
        return ObjectMapper.Map<TaskType, TaskTypeDto>(result);
    }

    [Authorize(VayuPermissions.TaskTypes.Edit)]
    public async Task<TaskTypeDto> UpdateAsync(TaskTypeCreateUpdateDto input)
    {
        var taskType = await _taskTypeRepository.GetAsync(input.Id);
        taskType.Name = input.Name;
        taskType.TaskCategory = input.TaskCategory;

        var result = await _taskTypeRepository.UpdateAsync(taskType);
        return ObjectMapper.Map<TaskType, TaskTypeDto>(result);
    }

    [Authorize(VayuPermissions.TaskTypes.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return _taskTypeRepository.DeleteAsync(id);
    }

    public Task<PagedResultDto<LookupDto<TaskCategory>>> GetTaskCategoryListAsync(LookupRequestDto input)
    {
        
        var taskCategories = Enum.GetValues(typeof(TaskCategory))
            .Cast<TaskCategory>()
            .WhereIf(input.Filter != null, x => _stringLocalize["Enum:TaskCategory:" + x.ToString()].ToString().Contains(input.Filter))
            .Select(s => new LookupDto<TaskCategory>()
            {
                Id = s,
                DisplayName = _stringLocalize["Enum:TaskCategory:" + s.ToString()]
            }).ToList();

        return Task.FromResult(new PagedResultDto<LookupDto<TaskCategory>>()
        {
            TotalCount = taskCategories.Count,
            Items = taskCategories
        });
    }
}