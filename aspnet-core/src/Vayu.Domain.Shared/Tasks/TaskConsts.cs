using System;
using System.Collections.Generic;
using System.Drawing;

namespace Vayu.Tasks;

public static class TaskConsts
{
    public const string EntityType = "Task";
    public static int MaxDescriptionLength { get; set; } = Int32.MaxValue;
    public static int MaxNoteLength { get; set; } = Int32.MaxValue;
    public static int MaxContactPersonLength { get; set; } = 128;
    public static int MaxContactPersonNumberLength { get; set; } = 30;
}

public class StatusIcon
{
    public string Initial => "fas fa-plus-circle";
    public string Todo => "fas fa-user-check";
    public string InProgress => "fas fa-people-carry";
    public string Suspended => "fas fa-ban";
    public string InReview => "fas fa-clipboard-check";
    public string Done => "fas fa-check-double";
    public string Cancel => "fas fa-times-circle";
}

public class StatusColor
{
    public Color Initial => Color.LightCoral;
    public Color Todo => Color.LightBlue;
    public Color InProgress => Color.SandyBrown;
    public Color Suspended => Color.DimGray;
    public Color InReview => Color.LightSeaGreen;
    public Color Done => Color.SeaGreen;
    public Color Cancel => Color.Crimson;
}