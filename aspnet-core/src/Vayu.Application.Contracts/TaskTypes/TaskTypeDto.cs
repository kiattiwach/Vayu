using System;
using Volo.Abp.Application.Dtos;

namespace Vayu.TaskTypes;

public class TaskTypeDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public TaskTypeEnum Type { get; set; }
}