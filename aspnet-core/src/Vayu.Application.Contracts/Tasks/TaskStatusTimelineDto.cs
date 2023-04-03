using JetBrains.Annotations;

namespace Vayu.Tasks;

public class TaskStatusTimelineDto
{
    [CanBeNull] public string Label { get; set; }
    [CanBeNull] public string Icon { get; set; }
    public delegate void Command(object commandEvent);
    [CanBeNull] public string Url { get; set; }
    [CanBeNull] public string RouterLink { get; set; }
    public bool? Expanded { get; set; }
    public bool? Disable { get; set; }
    public bool? Visible { get; set; }
    public bool? Active { get; set; }
    [CanBeNull] public object Style { get; set; }
    [CanBeNull] public string Color { get; set; }
    [CanBeNull] public string StyleClass { get; set; }
    [CanBeNull] public string Title { get; set; }
    [CanBeNull] public string Id { get; set; }

}