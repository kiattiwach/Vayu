using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Tasks;

public class Task : FullAuditedAggregateRoot<Guid>
{
    public string Description { get; set; }
    public Guid TaskTypeId { get; set; }
    public Status Status { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? CompleteDateTime { get; set; }
    [CanBeNull] public string Note { get; set; }
    public Guid LeaderId { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public List<TaskStaff> TaskStaffs { get; set; }

    public Task()
    {

    }
    public Task(string description, Guid taskTypeId, Status status, DateTime createDateTime, DateTime completeDateTime, string note, Guid leaderId, string contactPerson, string contactNumber)
    {
        Description = description;
        TaskTypeId = taskTypeId;
        Status = status;
        CreateDateTime = createDateTime;
        CompleteDateTime = completeDateTime;
        Note = note;
        LeaderId = leaderId;
        ContactPerson = contactPerson;
        ContactNumber = contactNumber;
    }
}