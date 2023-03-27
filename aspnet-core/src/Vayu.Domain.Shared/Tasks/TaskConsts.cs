using System;

namespace Vayu.Tasks;

public static class TaskConsts
{
    public const string EntityType = "Task";
    public static int MaxDescriptionLength { get; set; } = Int32.MaxValue;
    public static int MaxNoteLength { get; set; } = Int32.MaxValue;
    public static int MaxContactPersonLength { get; set; } = 128;
    public static int MaxContactPersonNumberLength { get; set; } = 30;
}