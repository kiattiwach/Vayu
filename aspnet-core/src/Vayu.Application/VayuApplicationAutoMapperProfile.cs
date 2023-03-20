using AutoMapper;
using Vayu.Roles;
using Vayu.Staffs;
using Vayu.Tasks;
using Vayu.TaskTypes;

namespace Vayu;

public class VayuApplicationAutoMapperProfile : Profile
{
    public VayuApplicationAutoMapperProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<Staff, StaffDto>();
        CreateMap<TaskType, TaskTypeDto>();
        CreateMap<Task, TaskDto>(MemberList.Destination);
    }
}
