using System;
using Volo.Abp.Application.Dtos;

namespace Vayu.Tasks;

public class TaskStaffFilterDto : EntityDto
{
    public Guid TaskId { get; set; }
    public Guid StaffId { get; set; }
}