using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Vayu.Tasks;

public interface ITaskService : IApplicationService
{
    Task<TaskDto> GetAsync(Guid id);
    Task<PagedResultDto<TaskWithNavigatePropertiesDto>> GetListAsync(TaskFilterDto input);
    Task<TaskDto> CreateAsync(TaskCreateUpdateDto input);
    Task<TaskDto> UpdateAsync(TaskCreateUpdateDto input);
    Task DeleteAsync(Guid id);
}