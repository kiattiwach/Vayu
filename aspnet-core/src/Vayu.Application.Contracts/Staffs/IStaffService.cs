using System;
using System.Threading.Tasks;
using Vayu.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Vayu.Staffs;

public interface IStaffService : IApplicationService
{
    Task<StaffDto> GetAsync(Guid id);

    Task<PagedResultDto<StaffWithNavigatePropertiesDto>> GetListAsync(StaffFilterDto input);

    Task<StaffDto> CreateAsync(StaffCreateUpdateDto input);

    Task<StaffDto> UpdateAsync(StaffCreateUpdateDto input);

    Task DeleteAsync(Guid id);

    Task<PagedResultDto<LookupDto<Guid>>> GetRoleLookupAsync(LookupRequestDto input);
    Task TestSendMailAsync(string to);
}