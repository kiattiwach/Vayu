using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper.Internal.Mappers;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Vayu.TaskTypes;

public class TaskTypeService : ApplicationService, ITaskTypeService
{
    private readonly ITaskTypeRepository _taskTypeRepository;

    public TaskTypeService(ITaskTypeRepository taskTypeRepository)
    {
        _taskTypeRepository = taskTypeRepository;
    }

    public async Task<TaskTypeDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<TaskType, TaskTypeDto>(await _taskTypeRepository.GetAsync(id));
    }

    public async Task<PagedResultDto<TaskTypeDto>> GetListAsync(TaskTypeFilterDto input)
    {
        var count = await _taskTypeRepository.GetCountAsync(input.filter, input.TaskCategory);
        var items = await _taskTypeRepository.GetListAsync(input.filter, input.TaskCategory);
        return new PagedResultDto<TaskTypeDto>
            { TotalCount = count, Items = ObjectMapper.Map<List<TaskType>, List<TaskTypeDto>>(items) };
    }

    public async Task<TaskTypeDto> CreateAsync(TaskTypeCreateUpdateDto input)
    {
        var result= await _taskTypeRepository.InsertAsync(new TaskType(input.Name, input.TaskCategory));
        return ObjectMapper.Map<TaskType, TaskTypeDto>(result);
    }

    public async Task<TaskTypeDto> UpdateAsync(TaskTypeCreateUpdateDto input)
    {
        var taskType = await _taskTypeRepository.GetAsync(input.Id);
        taskType.Name = input.Name;
        taskType.TaskCategory = input.TaskCategory;

        var result = await _taskTypeRepository.UpdateAsync(taskType);
        return ObjectMapper.Map<TaskType, TaskTypeDto>(result);
    }

    public Task DeleteAsync(Guid id)
    {
        return _taskTypeRepository.DeleteAsync(id);
    }
}