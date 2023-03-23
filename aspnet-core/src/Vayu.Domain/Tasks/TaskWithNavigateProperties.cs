using System.Collections.Generic;
using Vayu.Staffs;
using Vayu.TaskTypes;

namespace Vayu.Tasks;

public class TaskWithNavigateProperties
{
    public Task Task { get; set; }
    public TaskType Type { get; set; }
    public Staff Reader { get; set; }   
    public List<Staff> TeamMembers { get; set; }
}