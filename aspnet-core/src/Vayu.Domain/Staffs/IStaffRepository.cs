using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Vayu.Staffs;

public interface IStaffRepository : IBasicRepository<Staff,Guid>
{
    Task<int> GetCountAsync(string filter = null,
        string code = null, 
        string name = null,
        string surName = null,
        string codeName = null, 
        string email = null, 
        string address = null, 
        string radioCode = null, 
        string contactNumber = null, 
        string contactPerson = null, 
        string contactPersonNumber = null, 
        string line = null, 
        string facebook = null,
        DateTime? minStartDate = null,
        DateTime? maxStartDate = null,
        DateTime? minPassAssessmentDate = null,
        DateTime? maxPassAssessmentDate = null,
        Guid? roleId = null,
        CancellationToken cancellationToken = default);

    Task<List<StaffWithNavigateProperties>> GetListAsync(string filter = null,
        string code = null,
        string name = null,
        string surName = null,
        string codeName = null,
        string email = null,
        string address = null,
        string radioCode = null,
        string contactNumber = null,
        string contactPerson = null,
        string contactPersonNumber = null,
        string line = null,
        string facebook = null,
        DateTime? minStartDate = null,
        DateTime? maxStartDate = null,
        DateTime? minPassAssessmentDate = null,
        DateTime? maxPassAssessmentDate = null,
        Guid? roleId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);

    Task<Staff> GetByRadioCode(string radioCode, Guid? id = null);
    Task<Staff> GetByCodeName(string codeName, Guid? id = null);
    Task<Staff> GetByCode(string code, Guid? id = null);
}