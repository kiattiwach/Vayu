using System;
using System.Threading.Tasks;
using Vayu.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Vayu.Staffs;

public class StaffService : ApplicationService, IStaffService
{
    public Task<StaffDto> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResultDto<StaffDto>> GetListAsync(StaffFilterDto input)
    {
        throw new NotImplementedException();
    }

    public Task<StaffDto> CreateAsync(StaffCreateUpdateDto input)
    {
        throw new NotImplementedException();
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