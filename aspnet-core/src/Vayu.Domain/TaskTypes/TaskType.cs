using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.TaskTypes;

public class TaskType : FullAuditedEntity<Guid>
{
    public string Name { get; set; }
    public TaskTypeEnum Type { get; set; }

    public TaskType()
    {

    }
    public TaskType(string name, TaskTypeEnum type)
    {
        Name = name;
        Type = type;
    }
}