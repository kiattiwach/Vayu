using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Vayu.TaskTypes;

public class TaskTypeCreateUpdateDto : EntityDto<Guid>
{
    [Required]
    [DynamicMaxLength(typeof(TaskTypeConsts), nameof(TaskTypeConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    public TaskTypeEnum Type { get; set; }
}