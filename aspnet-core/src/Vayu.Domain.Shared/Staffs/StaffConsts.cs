using System;

namespace Vayu.Staffs;

public class StaffConsts
{
    public const string EntityType = "Staff";
    public static int MaxCodeLength { get; set; } = 16;
    public static int MaxNameLength { get; set; } = 128;
    public static int MaxSurNameLength { get; set; } = 128;
    public static int MaxCodeNameLength { get; set; } = 128;
    public static int MaxContactNumberLength { get; set; } = 30;
    public static int MaxAddressLength { get; set; } = 1024;
    public static int MaxRadioCodeLength { get; set; } = 32;
    public static int MaxEmailLength { get; set; } = 128;
    public static DateTime MinStartDate { get; set; } =
        (new DateTime(1973, 1, 1, 0, 0, 0, DateTimeKind.Utc)).ToLocalTime();

}