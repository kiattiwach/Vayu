using System;
using System.Threading.Tasks;
using Vayu.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Vayu.TaskTypes;

public interface ITaskTypeService : IApplicationService
{
    Task<TaskTypeDto> GetAsync(Guid id);

    Task<PagedResultDto<TaskTypeDto>> GetListAsync(TaskTypeFilterDto input);

    Task<TaskTypeDto> CreateAsync(TaskTypeCreateUpdateDto input);

    Task<TaskTypeDto> UpdateAsync(TaskTypeCreateUpdateDto input);

    Task DeleteAsync(Guid id);

    Task<PagedResultDto<LookupDto<TaskCategory>>> GetTaskCategoryListAsync(LookupRequestDto input);
}