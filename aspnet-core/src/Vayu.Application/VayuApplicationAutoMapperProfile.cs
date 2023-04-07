using System;
using AutoMapper;
using Vayu.Roles;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.Tasks;
using Vayu.TaskTypes;
using Volo.Abp.AutoMapper;

namespace Vayu;

public class VayuApplicationAutoMapperProfile : Profile
{
    public VayuApplicationAutoMapperProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<Role, LookupDto<Guid>>().ForMember(x => x.DisplayName, option => option.MapFrom(s => s.Name));
        CreateMap<Staff, StaffDto>();
        CreateMap<TaskType, TaskTypeDto>();
        CreateMap<Task, TaskDto>(MemberList.Destination)
            .ForMember(x => x.OverAllDuration, option => option.MapFrom(x => GetOverAllDuration(x)));
        CreateMap<Task, TaskViewDto>(MemberList.Destination)
            .Ignore(x => x.LatestStatusDuration)
            .Ignore(x => x.FinalStatus)
            .Ignore(x => x.OverAllDuration)
            .ForMember(x => x.OverAllTimeSpan, option => option.MapFrom(x => GetOverAllDuration(x)));
        CreateMap<TaskWithNavigateProperties, TaskWithNavigatePropertiesDto>()
            .ForMember(x => x.Staff, option => option.MapFrom(s => s.Reader))
            .Ignore(x => x.AvailableStatus);
        CreateMap<TaskStatus, TaskStatusDto>();
        CreateMap<TaskType, LookupDto<Guid>>().ForMember(x => x.DisplayName, option => option.MapFrom(s => s.Name));
        CreateMap<Staff, LookupDto<Guid>>().ForMember(x => x.DisplayName, option => option.MapFrom(s => s.Name));
        CreateMap<Staff, LookupDto<StaffDto>>().ForMember(x => x.DisplayName, option => option.MapFrom(s => s.Name));
    }

    private TimeSpan GetOverAllDuration(Task task)
    {
        return task.CompleteDateTime?.Subtract(task.CreateDateTime) ?? DateTime.Now.Subtract(task.CreateDateTime);
    }
}
