using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Tasks;

public class TaskStatus : AuditedEntity<Guid>
{
    public Guid TaskId { get; set; }
    public Status Status { get; set; }
}