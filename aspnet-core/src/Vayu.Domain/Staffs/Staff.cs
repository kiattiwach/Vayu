using System;
using JetBrains.Annotations;
using Vayu.Roles;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Staffs;

public class Staff : FullAuditedEntity<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string ContactNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? PassAssessmentDate { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string RadioCode { get; set; }
    public string ContactPerson { get; set; }
    public string ContactPersonNumber {get; set; }
    public string Line { get; set; }
    public string Facebook { get; set; }
    public Guid RoleId { get; set; }

    public Staff()
    {
    }

    public Staff(string code, string name, string codeName, string email, string address, string radioCode,
        string contactNumber, string contactPerson, string contactPersonNumber, string line, string facebook,
        DateTime startDate, DateTime? passAssessmentDate,
        Guid roleId)
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
        StartDate = startDate;
        PassAssessmentDate = passAssessmentDate;
    }
}