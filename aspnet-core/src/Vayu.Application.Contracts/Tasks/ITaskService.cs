using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vayu.Shared;
using Vayu.Staffs;
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
    Task<PagedResultDto<LookupDto<Guid>>> GetTaskTypeLookupAsync(LookupRequestDto input);
    Task<PagedResultDto<LookupDto<Guid>>> GetStaffLookupAsync(LookupRequestDto input);
    Task<List<StaffDto>> GetAvailableStaffsAsync(Guid[] ids);
    Task<TaskViewDto> GetTaskInfoAsync(Guid id);
    Task UpdateTaskStatusAsync(TaskStatusDto input);
    Task<List<StaffDto>> GetStaffManyAsync(List<Guid> ids);
    Task<PagedResultDto<LookupDto<Status>>> GetTaskStatusListAsync(LookupRequestDto input);
}