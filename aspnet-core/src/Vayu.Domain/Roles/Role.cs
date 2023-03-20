using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Vayu.Roles;

public class Role : FullAuditedEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }
}