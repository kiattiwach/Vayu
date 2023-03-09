using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Staffs;

public class Staff : AuditedEntity<Guid>
{
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string ContactNumber { get; set; }

    public Staff()
    {

    }
    public Staff(string name, string codeName, string contactNumber)
    {
        Name = name;
        CodeName = codeName;
        ContactNumber = contactNumber;
    }
}