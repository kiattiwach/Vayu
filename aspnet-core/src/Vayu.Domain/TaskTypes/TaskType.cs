using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.TaskTypes;

public class TaskType : FullAuditedEntity<Guid>
{
    public string Name { get; set; }
    public TaskCategory TaskCategory { get; set; }
    public TaskType(string name, TaskCategory taskCategory)
    {
        Name = name;
        TaskCategory = taskCategory;
    }
}