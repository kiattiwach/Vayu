using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Vayu.Permissions;
using Vayu.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace Vayu.Staffs;

public class StaffService : VayuAppService, IStaffService
{
    private readonly IStaffRepository _staffRepository;

    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public async Task<StaffDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Staff, StaffDto>(await _staffRepository.GetAsync(id));
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public async Task<PagedResultDto<StaffDto>> GetListAsync(StaffFilterDto input)
    {
        var count = await _staffRepository.GetCountAsync(input.Filter, input.Code, input.Name, input.CodeName,
            input.Email, input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson,
            input.ContactPersonNumber, input.Line, input.Facebook, input.RoleId);
        var items = await _staffRepository.GetListAsync(input.Filter, input.Code, input.Name, input.CodeName,
            input.Email, input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson,
            input.ContactPersonNumber, input.Line, input.Facebook, input.RoleId,input.MaxResultCount,input.SkipCount);
        return new PagedResultDto<StaffDto>(count, ObjectMapper.Map<List<Staff>, List<StaffDto>>(items));
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public Task<StaffDto> CreateAsync(StaffCreateUpdateDto input)
    {
        var result = await _staffRepository.InsertAsync(new Staff(input.Code, input.Name));
    }

    public Task<StaffDto> UpdateAsync(StaffCreateUpdateDto input)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResultDto<LookupDto<Guid>>> GetRoleLookupAsync(LookupRequestDto input)
    {
        throw new NotImplementedException();
    }
}