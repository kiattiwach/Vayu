using System;
using System.Collections.Generic;
using System.Drawing;
using Vayu.Shared;
using Vayu.Staffs;
using Vayu.TaskTypes;

namespace Vayu.Tasks;

public class TaskWithNavigatePropertiesDto
{
    private static Dictionary<Status, AvailableStatusDto[]> _availableStatus =
        new Dictionary<Status, AvailableStatusDto[]>()
        {
            {
                Status.Initial, new[]
                {
                    new AvailableStatusDto(Status.Todo),
                    new AvailableStatusDto(Status.Suspended, true),
                    new AvailableStatusDto(Status.Cancel, true)
                }
            },
            {
                Status.Todo, new[]
                {

                    new AvailableStatusDto(Status.InProgress),
                    new AvailableStatusDto(Status.Initial, true),
                    new AvailableStatusDto(Status.Suspended, true),
                    new AvailableStatusDto(Status.Cancel, true)
                }
            },
            {
                Status.InProgress, new[]
                {
                    new AvailableStatusDto(Status.InReview),
                    new AvailableStatusDto(Status.Todo, true),
                    new AvailableStatusDto(Status.Suspended, true),
                    new AvailableStatusDto(Status.Cancel, true)
                }
            },
            {
                Status.InReview, new[]
                {

                    new AvailableStatusDto(Status.Done),
                    new AvailableStatusDto(Status.InProgress, true),
                    new AvailableStatusDto(Status.Suspended, true),
                    new AvailableStatusDto(Status.Cancel, true)
                }
            },
            {
                Status.Done, new[]
                {
                    new AvailableStatusDto(Status.InReview, true),
                    new AvailableStatusDto(Status.Suspended, true),
                    new AvailableStatusDto(Status.Cancel, true)
                }
            },
            {
                Status.Suspended, new[]
                {
                    new AvailableStatusDto(Status.InReview, true)
                }
            },
            {
                Status.Cancel, new[]
                {
                    new AvailableStatusDto(Status.InReview, true)
                }
            }
        };

    public TaskDto Task { get; set; }
    public TaskTypeDto TaskType { get; set; }
    public StaffDto Staff { get; set; }
    public List<TaskStatusDto> TaskStatuses { get; set; }
    public AvailableStatusDto[] AvailableStatus => _availableStatus[Task.Status];
    public string StatusColor => Task.Status.GetColor();
    public string GroupKey => Task.CreateDateTime.ToShortDateString();
}