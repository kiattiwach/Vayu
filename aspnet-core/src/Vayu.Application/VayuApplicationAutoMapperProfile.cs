using System;
using AutoMapper;
using Vayu.Roles;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.Tasks;
using Vayu.TaskTypes;

namespace Vayu;

public class VayuApplicationAutoMapperProfile : Profile
{
    public VayuApplicationAutoMapperProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<Role, LookupDto<Guid>>().ForMember(x => x.DisplayName, option => option.MapFrom(s => s.Name));
        CreateMap<Staff, StaffDto>();
        CreateMap<TaskType, TaskTypeDto>();
        CreateMap<Task, TaskDto>(MemberList.Destination);
        CreateMap<TaskWithNavigateProperties, TaskWithNavigatePropertiesDto>()
            .ForMember(x => x.Staff, option => option.MapFrom(s => s.Reader));

    }
}
