using System;
using Volo.Abp.Application.Dtos;

namespace Vayu.Staffs;

public class StaffDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string ContactNumber { get; set; }
}