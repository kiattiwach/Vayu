using System;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace Vayu.Staffs;

public class StaffFilterDto : PagedAndSortedResultRequestDto
{
    [CanBeNull] public string Filter { get; set; }
    public Guid? RoleId { get; set; }
}