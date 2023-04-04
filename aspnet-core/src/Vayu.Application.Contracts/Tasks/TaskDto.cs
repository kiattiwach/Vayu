using System;
using System.Collections.Generic;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Vayu.Tasks;

public class TaskDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid TaskTypeId { get; set; }
    public Status Status { get; set; }
    public string Description { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? CompleteDateTime { get; set; }
    public string Note { get; set; }
    public Guid LeaderId { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public List<Guid> TeamMemberIds { get; set; }
    public string ConcurrencyStamp { get; set; }
    public TimeSpan OverAllDuration { get; set; }
}