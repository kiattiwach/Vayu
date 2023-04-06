using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Tasks;

public class TaskStatus : AuditedEntity<Guid>
{
    public Guid TaskId { get; set; }
    public Status Status { get; set; }
    public string Note { get; set; }

    public TaskStatus(Guid taskId, Status status, [CanBeNull] string note)
    {
        TaskId = taskId;
        Status = status;
        Note = note;
    }
}