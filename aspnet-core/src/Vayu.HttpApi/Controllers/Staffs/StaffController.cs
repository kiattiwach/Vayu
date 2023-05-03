using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vayu.Shared;
using Vayu.Staffs;
using Volo.Abp.Application.Dtos;

namespace Vayu.Controllers.Staffs;

[Route("api/staff")]
public class StaffController : VayuController, IStaffService
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet("get/{id}")]
    public Task<StaffDto> GetAsync(Guid id)
    {
        return _staffService.GetAsync(id);
    }

    [HttpGet("get-by-email")]
    public Task<StaffDto> GetByEmailAsync(string email)
    {
        return _staffService.GetByEmailAsync(email);
    }

    [HttpPost("get-list/{input}")]
    public Task<PagedResultDto<StaffWithNavigatePropertiesDto>> GetListAsync(StaffFilterDto input)
    {
        return _staffService.GetListAsync(input);
    }

    [HttpPost("create/{input}")]
    public Task<StaffDto> CreateAsync(StaffCreateUpdateDto input)
    {
        return _staffService.CreateAsync(input);
    }

    [HttpPost("update/{input}")]
    public Task<StaffDto> UpdateAsync(StaffCreateUpdateDto input)
    {
        return _staffService.UpdateAsync(input);
    }

    [HttpGet("delete/{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _staffService.DeleteAsync(id);
    }

    [HttpPost("get-role-lookup/{input}")]
    public Task<PagedResultDto<LookupDto<Guid>>> GetRoleLookupAsync(LookupRequestDto input)
    {
        return _staffService.GetRoleLookupAsync(input);
    }

    [HttpGet("test-send-mail/{to}")]
    public Task TestSendMailAsync(string to)
    {
        return _staffService.TestSendMailAsync(to);
    }
}