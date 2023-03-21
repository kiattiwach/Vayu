using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Vayu.Permissions;
using Vayu.Roles;
using Vayu.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Vayu.Staffs;

public class StaffService : VayuAppService, IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IRepository<Role, Guid> _roleRepository;

    public StaffService(IStaffRepository staffRepository, IRepository<Role,Guid> roleRepository)
    {
        _staffRepository = staffRepository;
        _roleRepository = roleRepository;
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

    [Authorize(VayuPermissions.Staffs.Create)]
    public async Task<StaffDto> CreateAsync(StaffCreateUpdateDto input)
    {
        var result = await _staffRepository.InsertAsync(new Staff(input.Code, input.Name, input.CodeName, input.Email,
            input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson, input.ContactPersonNumber,
            input.Line, input.Facebook, input.RoleId));
        return ObjectMapper.Map<Staff, StaffDto>(result);
    }

    [Authorize(VayuPermissions.Staffs.Edit)]
    public async Task<StaffDto> UpdateAsync(StaffCreateUpdateDto input)
    {
        var staff = await _staffRepository.GetAsync(input.Id);
        staff.Code = input.Code;
        staff.Name = input.Name;
        staff.CodeName = input.CodeName;
        staff.Email = input.Email;
        staff.Address = input.Address;
        staff.RadioCode = input.RadioCode;
        staff.ContactNumber = input.ContactNumber;
        staff.ContactPerson = input.ContactPerson;
        staff.ContactPersonNumber = input.ContactPersonNumber;
        staff.Line = input.Line;
        staff.Facebook = input.Facebook;
        staff.RoleId = input.RoleId;

        var result = await _staffRepository.UpdateAsync(staff);
        return ObjectMapper.Map<Staff,StaffDto>(result);
    }

    [Authorize(VayuPermissions.Staffs.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return _staffRepository.DeleteAsync(id);
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public async Task<PagedResultDto<LookupDto<Guid>>> GetRoleLookupAsync(LookupRequestDto input)
    {
        var query = (await _roleRepository.GetQueryableAsync())
            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                x => x.Name != null &&x.Name.Contains(input.Filter));
        var items = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Role>();

        return new PagedResultDto<LookupDto<Guid>>(items.Count,
            ObjectMapper.Map<List<Role>, List<LookupDto<Guid>>>(items));
    }
}