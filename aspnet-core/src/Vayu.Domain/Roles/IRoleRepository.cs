using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vayu.Roles;
using Volo.Abp.Domain.Repositories;

namespace Vayu;

public interface IRoleRepository : IBasicRepository<Role, Guid>
{
    Task<int> GetCountAsync(string filter = null,
        string name = null,
        string description = null,
        CancellationToken cancellationToken = default);

    Task<List<Role>> GetListAsync(
        string filter = null,
        string name = null,
        string description = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);
}