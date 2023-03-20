using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace Vayu.Roles;

public class RoleFilterDto : PagedAndSortedResultRequestDto
{
    [CanBeNull] public string Filter { get; set; }
    [CanBeNull] public string Name { get; set; }
    [CanBeNull] public string Description { get; set; }
}