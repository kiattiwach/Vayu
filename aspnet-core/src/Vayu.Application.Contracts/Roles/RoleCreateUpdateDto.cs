using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Vayu.Roles;

public class RoleCreateUpdateDto : EntityDto<Guid>
{
    [DynamicMaxLength(typeof(RoleConsts), nameof(RoleConsts.MaxNameLength))]
    [Required]
    public string Name { get; set; }

    [DynamicMaxLength(typeof(RoleConsts), nameof(RoleConsts.MaxDescriptionLength))]
    public string Description { get; set; }
}