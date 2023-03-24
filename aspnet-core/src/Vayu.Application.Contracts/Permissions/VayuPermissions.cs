namespace Vayu.Permissions;

public class VayuPermissions
{
    public const string GroupName = "Vayu";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class TaskTypes
    {
        public const string Default = GroupName + ".TaskTypes";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Roles
    {
        public const string Default = GroupName + ".Roles";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Staffs
    {
        public const string Default = GroupName + ".Staffs";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Tasks
    {
        public const string Default = GroupName + ".Tasks";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Todo = Default + ".Todo";
        public const string InProgress = Default + ".InProgress";
        public const string Suspended = Default + ".Suspended";
        public const string InReview = Default + ".InReview";
        public const string Done = Default + ".Done";
        public const string Cancel = Default + ".Cancel";
    }
}
