using Volo.Abp.Application.Dtos;

namespace Vayu.TaskTypes;

public class TaskTypeFilterDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    public TaskCategory? TaskCategory { get; set; }
}