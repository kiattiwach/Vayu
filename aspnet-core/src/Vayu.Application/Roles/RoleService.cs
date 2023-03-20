using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Vayu.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Vayu.Roles;

public class RoleService : ApplicationService, IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [Authorize(VayuPermissions.Roles.Default)]
    public async Task<RoleDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Role, RoleDto>(await _roleRepository.GetAsync(id));
    }

    [Authorize(VayuPermissions.Roles.Default)]
    public async Task<PagedResultDto<RoleDto>> GetListAsync(RoleFilterDto input)
    {
        var count = await _roleRepository.GetCountAsync(input.Filter, input.Name, input.Description);
        var items = await _roleRepository.GetListAsync(input.Filter, input.Name, input.Description,
            input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<RoleDto>
            { TotalCount = count, Items = ObjectMapper.Map<List<Role>, List<RoleDto>>(items) };
    }

    [Authorize(VayuPermissions.Roles.Create)]
    public async Task<RoleDto> CreateAsync(RoleCreateUpdateDto input)
    {
        var result = await _roleRepository.InsertAsync(new Role(input.Name, input.Description));
        return ObjectMapper.Map<Role, RoleDto>(result);
    }

    [Authorize(VayuPermissions.Roles.Edit)]
    public async Task<RoleDto> UpdateAsync(RoleCreateUpdateDto input)
    {
        var role = await _roleRepository.GetAsync(input.Id);
        role.Name = input.Name;
        role.Description = input.Description;

        var result = await _roleRepository.UpdateAsync(role);
        return ObjectMapper.Map<Role, RoleDto>(result);
    }

    [Authorize(VayuPermissions.Roles.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return  _roleRepository.DeleteAsync(id);
    }
}