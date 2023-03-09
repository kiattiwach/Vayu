using Volo.Abp.Application.Dtos;

namespace Vayu.TaskTypes;

public class TaskTypeFilterDto : PagedAndSortedResultRequestDto
{
    public string filter { get; set; }
    public TaskTypeEnum Type { get; set; }
}