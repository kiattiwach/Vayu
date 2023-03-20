using System;
using Volo.Abp.Application.Dtos;

namespace Vayu.Roles;

public class RoleDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
}