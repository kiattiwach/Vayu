using System;
using Volo.Abp.Domain.Entities;

namespace Vayu.Tasks;

public class TaskStaff : Entity
{
    public Guid TaskId { get; set; }
    public Guid StaffId { get; set; }
    public override object[] GetKeys()
    {
        return new object[]
        {
            TaskId,StaffId
        };
    }
}