using System;
using System.Collections.Generic;
using Vayu.Staffs;
using Vayu.TaskTypes;
using Volo.Abp.Application.Dtos;

namespace Vayu.Tasks;

public class TaskViewDto : EntityDto<Guid>
{
    public TaskTypeDto TaskType { get; set; }
    public Status ? Status { get; set; }
    public string Description { get; set; }
    public DateTime? CreateDateTime { get; set;}
    public DateTime? CompleteDateTime { get;set; }
    public string Note { get; set; }
    public StaffDto Leader { get; set;}
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public List<StaffDto> TeamMembers { get; set; }
    public List<TaskStatusTimelineDto> StatusTimeliness { get; set; }
}