using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Vayu.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Roles;

public class RoleRepository : EfCoreRepository<VayuDbContext, Role, Guid>, IRoleRepository
{
    public RoleRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<int> GetCountAsync(string filter = null, string name = null,
        string description = null, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQuery(filter, name, description);
        return await queryable.CountAsync(GetCancellationToken(cancellationToken)); ;
    }

    public  async Task<List<Role>> GetListAsync(string filter = null, string name = null,
        string description = null, int maxResultCount = Int32.MaxValue, int skipCount = 0, string sorting = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQuery(filter, name, description);
        queryable = queryable.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Role.Name)} desc" : sorting);
        return await queryable.Skip(skipCount)  
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    private async Task<IQueryable<Role>> GetQuery(string filter = null, string name = null, string description = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(!string.IsNullOrEmpty(name), x => x.Name.Contains(name))
            .WhereIf(!string.IsNullOrEmpty(description), x => x.Description.Contains(description));
    }
}