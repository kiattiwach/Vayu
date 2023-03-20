using System;

namespace Vayu.Roles;

public class RoleConsts
{
    public const string EntityType = "Role";
    public static int MaxNameLength { get; set; } = 128;
    public static int MaxDescriptionLength { get; set; } = Int32.MaxValue;
}