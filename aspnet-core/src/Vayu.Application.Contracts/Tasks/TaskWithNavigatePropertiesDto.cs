using System.Collections.Generic;
using Vayu.Staffs;
using Vayu.TaskTypes;

namespace Vayu.Tasks;

public class TaskWithNavigatePropertiesDto
{
    public TaskDto Task { get; set; }
    public TaskTypeDto TaskType { get; set; }
    public StaffDto Staff { get; set; }
    public List<StaffDto> TeamMembers { get; set; }
}