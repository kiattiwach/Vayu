using System;

namespace Vayu.Tasks;

public class AvailableStatusDto
{
    public Status Status { get; set; }
    public bool RequiredNote { get; set; }

    public AvailableStatusDto(Status status, bool requiredNote = false)
    {
        Status = status;
        RequiredNote = requiredNote;
    }
}