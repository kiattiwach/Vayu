using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Vayu.Roles;

public interface IRoleService : IApplicationService
{
    Task<RoleDto> GetAsync(Guid id);

    Task<PagedResultDto<RoleDto>> GetListAsync(RoleFilterDto input);

    Task<RoleDto> CreateAsync(RoleCreateUpdateDto input);

    Task<RoleDto> UpdateAsync(RoleCreateUpdateDto input);

    Task DeleteAsync(Guid id);
}