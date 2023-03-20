using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vayu.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Staffs;

public class StaffRepository : EfCoreRepository<VayuDbContext, Staff,Guid>, IStaffRepository
{
    public StaffRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<int> GetCountAsync(string filter = null, Guid? roleId = default, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Staff>> GetListAsync(string filter = null, Guid? roleId = default, int maxResultCount = Int32.MaxValue, int skipCount = 0,
        string sorting = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}