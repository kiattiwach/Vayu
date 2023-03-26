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
using PasswordGenerator;
using Vayu.Localization;
using Volo.Abp;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Volo.Abp.Emailing;

namespace Vayu.Staffs;

public class StaffService : VayuAppService, IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IRepository<Role, Guid> _roleRepository;
    private readonly IStringLocalizer<VayuResource> _stringLocalize;
    private readonly IRepository<IdentityUser, Guid> _identityRepository;
    private readonly IdentityUserManager _identityUserManager;
    private readonly IRepository<IdentityRole, Guid> _identityRoleRepository;
    private readonly IEmailSender _emailSender;
    private readonly IPassword _password;

    public StaffService(IStaffRepository staffRepository, 
        IRepository<Role,Guid> roleRepository,
        IStringLocalizer<VayuResource> stringLocalize,
        IRepository<IdentityUser, Guid> identityRepository,
        IdentityUserManager identityUserManager,
        IPassword password,
        IRepository<IdentityRole, Guid> identityRoleRepository,
        IEmailSender emailSender)

    {
        _staffRepository = staffRepository;
        _roleRepository = roleRepository;
        _stringLocalize = stringLocalize;
        _identityRepository = identityRepository;
        _identityUserManager = identityUserManager;
        _identityRoleRepository = identityRoleRepository;
        _emailSender = emailSender;
        _password = password.LengthRequired(8)
                            .IncludeUppercase()
                            .IncludeLowercase()
                            .IncludeNumeric()
                            .IncludeSpecial(); ;
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public async Task<StaffDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Staff, StaffDto>(await _staffRepository.GetAsync(id));
    }

    [Authorize(VayuPermissions.Staffs.Default)]
    public async Task<PagedResultDto<StaffWithNavigatePropertiesDto>> GetListAsync(StaffFilterDto input)
    {
        var count = await _staffRepository.GetCountAsync(input.Filter, input.Code, input.Name, input.SurName, input.CodeName,
            input.Email, input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson,
            input.ContactPersonNumber, input.Line, input.Facebook,
            input.MinStartDate, input.MaxStartDate, input.MinPassAssessmentDate, input.MaxPassAssessmentDate,
            input.RoleId);
        var items = await _staffRepository.GetListAsync(input.Filter, input.Code, input.Name, input.SurName, input.CodeName,
            input.Email, input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson,
            input.ContactPersonNumber, input.Line, input.Facebook,
            input.MinStartDate, input.MaxStartDate, input.MinPassAssessmentDate, input.MaxPassAssessmentDate,
            input.RoleId,input.MaxResultCount,input.SkipCount, input.Sorting);

        return new PagedResultDto<StaffWithNavigatePropertiesDto>(count, items.Select(s =>
            new StaffWithNavigatePropertiesDto()
            {
                Staff = ObjectMapper.Map<Staff, StaffDto>(s.Staff),
                Role = ObjectMapper.Map<Role, RoleDto>(s.Role),

            }).ToList());
    }

    [Authorize(VayuPermissions.Staffs.Create)]
    public async Task<StaffDto> CreateAsync(StaffCreateUpdateDto input)
    {
        await CheckUnUsedCode(input.Code);
        await CheckUnUsedRadioCode(input.RadioCode);
        await CheckUnUsedCodeName(input.CodeName);

        var userWithSameEmail = await _identityUserManager.FindByEmailAsync(input.Email);

        if (userWithSameEmail != null)
        {
            throw new UserFriendlyException(_stringLocalize["EmailIsUsed", input.Email]);
        }

        string password = _password.Next();
        var defaultRole = await _identityRoleRepository.SingleAsync(r => r.IsDefault);
        var identityUser = new IdentityUser(GuidGenerator.Create(), input.Email, input.Email)
        {
            Name = input.Name,
            Surname = input.Surname
        };
        
        identityUser.AddRole(defaultRole.Id);
        await _identityUserManager.CreateAsync(identityUser, password, true);
        input.UserId = identityUser.Id;

        var emailBody = $"{_stringLocalize["RegisterStaffInfo"]}<br>" + 
                       $"{_stringLocalize["Name"]}: {input.Name} <br>" +
                       $"{_stringLocalize["UserName"]}: {input.Email} <br>" +
                       $"{_stringLocalize["Password"]}: {password} <br>" + "<br>" +
                       $"<a href='https://www.skapp.at/Account/Login'{_stringLocalize["ClickHereToLogin {0}", VayuRemoteServiceConsts.ModuleName]}></a>";

        await _emailSender.SendAsync(input.Email, _stringLocalize["RegisterMailSubject"], emailBody, true);

        var result = await _staffRepository.InsertAsync(new Staff(input.Code, input.Name,input.Surname, input.CodeName, input.Email,
            input.Address, input.RadioCode, input.ContactNumber, input.ContactPerson, input.ContactPersonNumber,
            input.Line, input.Facebook,input.StartDate, input.PassAssessmentDate, input.RoleId, input.UserId));
        
        return ObjectMapper.Map<Staff, StaffDto>(result);
    }

    [Authorize(VayuPermissions.Staffs.Edit)]
    public async Task<StaffDto> UpdateAsync(StaffCreateUpdateDto input)
    {
        await CheckUnUsedCodeName(input.CodeName, input.Id);
        await CheckUnUsedRadioCode(input.RadioCode, input.Id);

        var staff = await _staffRepository.GetAsync(input.Id);
        staff.Code = input.Code;
        staff.Name = input.Name;
        staff.SurName = input.Surname;
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
        staff.StartDate = input.StartDate;
        staff.PassAssessmentDate = input.PassAssessmentDate;

        var result = await _staffRepository.UpdateAsync(staff);
        
        return ObjectMapper.Map<Staff, StaffDto>(result);
    }

    private async Task CheckUnUsedRadioCode(string radioCode, Guid? id = null)
    {
        var staffUsedRadioCode = await _staffRepository.GetByRadioCode(radioCode, id);
        if (staffUsedRadioCode != null)
        {
            throw new UserFriendlyException(
                _stringLocalize["RadioCodeIsUsed", staffUsedRadioCode.Name, radioCode]);
        }
    }

    private async Task CheckUnUsedCodeName(string codeName, Guid? id = null)
    {
        var staffUsedCodeName = await _staffRepository.GetByCodeName(codeName, id);
        if (staffUsedCodeName != null)
        {
            throw new UserFriendlyException(
                _stringLocalize["CodeNameIsUsed", staffUsedCodeName.Name, codeName]);
        }
    }

    private async Task CheckUnUsedCode(string code, Guid? id = null)
    {
        var staffUsedCodeName = await _staffRepository.GetByCode(code, id);
        if (staffUsedCodeName != null)
        {
            throw new UserFriendlyException(
                _stringLocalize["CodeIsUsed", staffUsedCodeName.Name, code]);
        }
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