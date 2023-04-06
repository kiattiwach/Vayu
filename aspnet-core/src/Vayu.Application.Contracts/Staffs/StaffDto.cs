using System;
using Vayu.Roles;
using Volo.Abp.Application.Dtos;

namespace Vayu.Staffs;

public class StaffDto : AuditedEntityDto<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string CodeName { get; set; }    
    public string ContactNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? PassAssessmentDate { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string RadioCode { get; set; }
    public string ContactPerson { get; set; }
    public string ContactPersonNumber { get; set; }
    public string Line { get; set; }
    public string Facebook { get; set; }
    public Guid RoleId { get; set; }
    public Guid? UserId { get; set; }
}