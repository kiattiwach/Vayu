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
using Microsoft.Extensions.Localization;
using Vayu.Localization;
using Volo.Abp;
using Volo.Abp.ObjectMapping;

namespace Vayu.Staffs;

public class StaffService : VayuAppService, IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IRepository<Role, Guid> _roleRepository;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;

    public StaffService(IStaffRepository staffRepository, 
        IRepository<Role,Guid> roleRepository,
        IStringLocalizer<VayuResource> stringLocalize)
    {
        _staffRepository = staffRepository;
        _roleRepository = roleRepository;
        _stringLocalize = stringLocalize;
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public async Task<StaffDto> GetAsync(Guid id)
    {
        var staff = await _staffRepository.GetAsync(id);
        var staffDto = ObjectMapper.Map<Staff, StaffDto>(staff);
        staffDto.Role = await GetStaffRole(staff);
        return staffDto;
    }

    private async Task<RoleDto> GetStaffRole(Staff staff)
    {
        return ObjectMapper.Map<Role, RoleDto>(await _roleRepository.GetAsync(staff.RoleId));
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
        
        return new PagedResultDto<StaffDto>(count, items.Select(s =>
        {
            var staff = ObjectMapper.Map<Staff, StaffDto>(s.Staff);
            staff.Role = ObjectMapper.Map<Role, RoleDto>(s.Role);
            return staff;
        }).ToList());
    }

    [Authorize(VayuPermissions.Staffs.Create)]
    public async Task<StaffDto> CreateAsync(StaffCreateUpdateDto input)
    {
        var staffUsedRadioCode = await _staffRepository.GetByRadioCode(input.RadioCode);
        if (staffUsedRadioCode != null)
        {
            throw new UserFriendlyException(
                _stringLocalize["RadioCodeIsUsed", staffUsedRadioCode.Name, input.RadioCode]);
        }

        var result = await _staffRepository.InsertAsync(new Staff(input.Code, input.Name, input.CodeName, input.Email,
            input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson, input.ContactPersonNumber,
            input.Line, input.Facebook, input.RoleId));
        var staff = ObjectMapper.Map<Staff, StaffDto>(result);
        staff.Role = await GetStaffRole(result);
        return staff;
    }

    [Authorize(VayuPermissions.Staffs.Edit)]
    public async Task<StaffDto> UpdateAsync(StaffCreateUpdateDto input)
    {
        var staffUsedRadioCode = await _staffRepository.GetByRadioCode(input.RadioCode);
        if (staffUsedRadioCode != null && !staffUsedRadioCode.Name.Equals(input.Name))
        {
            throw new UserFriendlyException(
                _stringLocalize["RadioCodeIsUsed", staffUsedRadioCode.Name, input.RadioCode]);
        }

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
        var staffDto = ObjectMapper.Map<Staff,StaffDto>(result);
        staffDto.Role = await GetStaffRole(result);
        return staffDto;
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