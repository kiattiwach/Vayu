using System;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace Vayu.Tasks;

public class TaskStatusDto : EntityDto<Guid>
{
    public Guid TaskId { get; set; }
    public Status Status { get; set; }
    [CanBeNull] public string Note { get; set; }
}