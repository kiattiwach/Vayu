using System;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace Vayu.Staffs;

public class StaffFilterDto : PagedAndSortedResultRequestDto
{
    [CanBeNull] public string Filter { get; set; }
    [CanBeNull]public string Code { get; set; }
    [CanBeNull]public string Name { get; set; }
    [CanBeNull] public string SurName { get; set; }
    [CanBeNull]public string CodeName { get; set; }
    [CanBeNull]public string Email { get; set; }
    [CanBeNull]public string Address { get; set; }
    [CanBeNull]public string RadioCode { get; set; }
    [CanBeNull]public string ContactNumber { get; set; }
    [CanBeNull]public string ContactPerson { get; set; }
    [CanBeNull]public string ContactPersonNumber { get; set; }
    [CanBeNull]public string Line { get; set; }
    [CanBeNull]public string Facebook { get; set; }
    [CanBeNull] public Guid RoleId { get; set; }
    public DateTime? MinStartDate { get; set; }
    public DateTime? MaxStartDate { get; set; }
    public DateTime? MinPassAssessmentDate { get; set; }
    public DateTime? MaxPassAssessmentDate { get; set; }
}