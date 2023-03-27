using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vayu.EntityFrameworkCore;
using Vayu.Roles;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Vayu.Staffs;

public class StaffRepository : EfCoreRepository<VayuDbContext, Staff,Guid>, IStaffRepository
{
    public StaffRepository(IDbContextProvider<VayuDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<int> GetCountAsync(string filter = null, string code = null, string name = null, string surName = null, string codeName = null,
        string email = null, string address = null, string radioCode = null, string contactNumber = null,
        string contactPerson = null, string contactPersonNumber = null, string line = null, string facebook = null,
        DateTime? minStartDate = null, DateTime? maxStartDate = null, DateTime? minPassAssessmentDate = null,
        DateTime? maxPassAssessmentDate = null, Guid? roleId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQuery(filter, code, name, surName,codeName, email, address, radioCode, contactNumber,
            contactPerson, contactPersonNumber, line, facebook, minStartDate, maxStartDate, minPassAssessmentDate,
            maxPassAssessmentDate, roleId);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<StaffWithNavigateProperties>> GetListAsync(string filter = null, string code = null, string name = null, string surName = null, string codeName = null,
        string email = null, string address = null, string radioCode = null, string contactNumber = null,
        string contactPerson = null, string contactPersonNumber = null, string line = null, string facebook = null,
        DateTime? minStartDate = null, DateTime? maxStartDate = null, DateTime? minPassAssessmentDate = null,
        DateTime? maxPassAssessmentDate = null, Guid? roleId = null, int maxResultCount = Int32.MaxValue, int skipCount = 0,
        string sorting = null, CancellationToken cancellationToken = default)
    {
        var query = await GetQuery(filter, code, name, surName, codeName, email, address, radioCode, contactNumber,
            contactPerson, contactPersonNumber, line, facebook, minStartDate, maxStartDate, minPassAssessmentDate,
            maxPassAssessmentDate, roleId);
        query = sorting.IsNullOrWhiteSpace() ? 
            query.OrderByDescending(x => x.Staff.CreationTime):
            query.OrderBy(sorting);
        return await query.Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
    }

    public async Task<Staff> GetByRadioCode(string radioCode, Guid? id = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(id != null, x => x.Id != id)
            .FirstOrDefault(x => x.RadioCode.Equals(radioCode));
    }

    public async Task<Staff> GetByCodeName(string codeName, Guid? id = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(id != null, x => x.Id != id)
            .FirstOrDefault(x => x.CodeName.Equals(codeName));

    }

    public async Task<Staff> GetByCode(string code, Guid? id = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(id != null, x => x.Id != id)
            .FirstOrDefault(x => x.Code.Equals(code));
    }

    public async Task<IQueryable<StaffWithNavigateProperties>> GetQuery(string filter = null, string code = null, string name = null, 
        string surName = null, string codeName = null,
        string email = null, string address = null, string radioCode = null, string contactNumber = null,
        string contactPerson = null, string contactPersonNumber = null, string line = null, string facebook = null,
        DateTime? minStartDate = null, DateTime? maxStartDate = null, DateTime? minPassAssessmentDate = null,
        DateTime? maxPassAssessmentDate = null,
        Guid? roleId = default)
    {
        var staffQuery = (await GetDbSetAsync())
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x =>
                x.Code.Contains(filter) || x.Name.Contains(filter) || x.Surname.Contains(filter) ||
                x.CodeName.Contains(filter) ||
                x.Email.Contains(filter) || x.Address.Contains(filter) || x.RadioCode.Contains(filter) ||
                x.ContactNumber.Contains(filter)
                || x.ContactPerson.Contains(filter) || x.ContactPersonNumber.Contains(filter) ||
                GetDbContextAsync().Result.VayuRoles.Any(r => r.Id == x.RoleId && r.Name.Contains(filter)))
            .WhereIf(!string.IsNullOrWhiteSpace(code), x => x.Code.Contains(code))
            .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name.Contains(name))
            .WhereIf(!string.IsNullOrWhiteSpace(surName), x => x.Surname.Contains(surName))
            .WhereIf(!string.IsNullOrWhiteSpace(codeName), x => x.CodeName.Contains(codeName))
            .WhereIf(!string.IsNullOrWhiteSpace(email), x => x.Email.Contains(email))
            .WhereIf(!string.IsNullOrWhiteSpace(address), x => x.Address.Contains(address))
            .WhereIf(!string.IsNullOrWhiteSpace(radioCode), x => x.RadioCode.Contains(radioCode))
            .WhereIf(!string.IsNullOrWhiteSpace(contactNumber), x => x.ContactNumber.Contains(contactNumber))
            .WhereIf(!string.IsNullOrWhiteSpace(contactPerson), x => x.ContactPerson.Contains(contactPerson))
            .WhereIf(!string.IsNullOrWhiteSpace(contactPersonNumber), x => x.ContactPersonNumber.Contains(contactPersonNumber))
            .WhereIf(!string.IsNullOrWhiteSpace(line), x => x.Line.Contains(line))
            .WhereIf(!string.IsNullOrWhiteSpace(facebook), x => x.Facebook.Contains(facebook))
            .WhereIf(roleId != Guid.Empty && roleId is { }, x => x.RoleId == roleId)
            .WhereIf(minStartDate != null, x => x.StartDate >= minStartDate)
            .WhereIf(maxStartDate != null, x => x.StartDate <= maxStartDate)
            .WhereIf(minPassAssessmentDate != null, x => x.PassAssessmentDate >= minPassAssessmentDate)
            .WhereIf(maxPassAssessmentDate!= null, x => x.PassAssessmentDate <= maxPassAssessmentDate);

        var result =  from staff in staffQuery
            join role in (await GetDbContextAsync()).Set<Role>() on staff.RoleId equals role.Id
            select new StaffWithNavigateProperties()
            {
                Staff = staff,
                Role = role
            };

        return result;
    }
}