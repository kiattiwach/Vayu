using System;

namespace Vayu.Staffs;

public class StaffConsts
{
    public const string EntityType = "Staff";
    public static int MaxNameLength { get; set; } = 512;
    public static int MaxCodeNameLength { get; set; } = 128;
    public static int MaxContactNumberLength { get; set; } = 30;
}