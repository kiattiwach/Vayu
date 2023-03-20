using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.Roles;
using Volo.Abp.Application.Dtos;

namespace Vayu.Controllers.Roles;

[Route("api/role")]
public class RoleController : VayuController, IRoleService
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("get/{id}")]
    public Task<RoleDto> GetAsync(Guid id)
    {
        return _roleService.GetAsync(id);
    }

    [HttpPost("get-list/{input}")]
    public Task<PagedResultDto<RoleDto>> GetListAsync(RoleFilterDto input)
    {
        return _roleService.GetListAsync(input);
    }
    
    [HttpPost("create/{input}")]
    public Task<RoleDto> CreateAsync(RoleCreateUpdateDto input)
    {
        return _roleService.CreateAsync(input);
    }

    [HttpPost("update/{input}")]
    public Task<RoleDto> UpdateAsync(RoleCreateUpdateDto input)
    {
        return _roleService.UpdateAsync(input);
    }

    [HttpGet("delete/{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _roleService.DeleteAsync(id);
    }
}