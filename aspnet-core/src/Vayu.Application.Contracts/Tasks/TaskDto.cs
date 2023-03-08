using System;
using System.Collections.Generic;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Vayu.Tasks;

public class TaskDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskTypeDto TaskType { get; set; }
    public Status Status { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime CompleteDateTime { get; set; }
    public string Note { get; set; }
    public StaffDto Staff { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public List<StaffDto> TaskStaffs { get; set; }
    public string ConcurrencyStamp { get; set; }
}