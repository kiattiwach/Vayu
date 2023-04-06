using Vayu.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Vayu.Permissions;

public class VayuPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var vayuGroup = context.AddGroup(VayuPermissions.GroupName);

        vayuGroup.AddPermission(VayuPermissions.Setting.Default, L("Permission:Settings"));

        var taskTypePermission = vayuGroup.AddPermission(VayuPermissions.TaskTypes.Default, L("Permission:TaskTypes"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Create, L("Permission:TaskTypes.Create"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Edit, L("Permission:TaskTypes.Edit"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Delete, L("Permission:TaskTypes.Delete"));

        var rolePermission = vayuGroup.AddPermission(VayuPermissions.Roles.Default, L("Permission:Roles"));
        rolePermission.AddChild(VayuPermissions.Roles.Create, L("Permission:Roles.Create"));
        rolePermission.AddChild(VayuPermissions.Roles.Edit, L("Permission:Roles.Edit"));
        rolePermission.AddChild(VayuPermissions.Roles.Delete, L("Permission:Roles.Delete"));

        var staffPermission = vayuGroup.AddPermission(VayuPermissions.Staffs.Default, L("Permission:Staffs"));
        staffPermission.AddChild(VayuPermissions.Staffs.Create, L("Permission:Staffs.Create"));
        staffPermission.AddChild(VayuPermissions.Staffs.Edit, L("Permission:Staffs.Edit"));
        staffPermission.AddChild(VayuPermissions.Staffs.Delete, L("Permission:Staffs.Delete"));

        var taskPermission = vayuGroup.AddPermission(VayuPermissions.Tasks.Default, L("Permission:Tasks"));
        taskPermission.AddChild(VayuPermissions.Tasks.Create, L("Permission:Tasks.Create"));
        taskPermission.AddChild(VayuPermissions.Tasks.Edit, L("Permission:Tasks.Edit"));
        taskPermission.AddChild(VayuPermissions.Tasks.Delete, L("Permission:Tasks.Delete"));
        taskPermission.AddChild(VayuPermissions.Tasks.UpdateStatus, L("Permission:Tasks.UpdateStatus"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VayuResource>(name);
    }
}
