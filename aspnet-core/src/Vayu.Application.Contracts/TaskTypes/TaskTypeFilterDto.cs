using Volo.Abp.Application.Dtos;

namespace Vayu.TaskTypes;

public class TaskTypeFilterDto : PagedAndSortedResultRequestDto
{
    public string filter { get; set; }
    public TaskCategory TaskCategory { get; set; }
}