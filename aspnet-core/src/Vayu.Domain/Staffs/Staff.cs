using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Staffs;

public class Staff : AuditedEntity<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string CodeName { get; set; }
    [CanBeNull] public string Email { get; set; }
    [CanBeNull] public string Address { get; set; }
    [CanBeNull] public string RadioCode { get; set; }
    [CanBeNull] public string ContactNumber { get; set; }
    [CanBeNull] public string ContactPerson { get; set; }
    [CanBeNull] public string ContactPersonNumber {get; set; }
    [CanBeNull] public string Line { get; set; }
    [CanBeNull] public string Facebook { get; set; }

    public Guid RoleId { get; set; }

    public Staff()
    {

    }
    public Staff(string code, string name, string codeName, string email, string address, string radioCode, string contactNumber, string contactPerson, string contactPersonNumber, string line, string facebook, Guid roleId)
    {
        Code = code;
        Name = name;
        CodeName = codeName;
        Email = email;
        Address = address;
        RadioCode = radioCode;
        ContactNumber = contactNumber;
        ContactPerson = contactPerson;
        ContactPersonNumber = contactPersonNumber;
        Line = line;
        Facebook = facebook;
        RoleId = roleId;
    }
}