using JetBrains.Annotations;
using System;
using Volo.Abp.Application.Dtos;

namespace Vayu.Tasks;

public class TaskFilterDto : EntityDto<Guid>
{
    [CanBeNull] public string Filter { get; set; }
    [CanBeNull] public string Name { get; set; }
    [CanBeNull] public string Description { get; set; }
    [CanBeNull] public Guid? TaskTypeId { get; set; }
    public Status? Status { get; set; }
    public DateTime? CreateDateTime { get; set; }
    public DateTime? CompleteDateTime { get; set; }
    [CanBeNull] public string Note { get; set; }
    [CanBeNull] public Guid? StaffId { get; set; }
    [CanBeNull] public string ContactPerson { get; set; }
    [CanBeNull] public string ContactNumber { get; set; }
}