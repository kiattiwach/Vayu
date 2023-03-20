using Vayu.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Vayu.Permissions;

public class VayuPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var vayuGroup = context.AddGroup(VayuPermissions.GroupName);
     
        var taskTypePermission = vayuGroup.AddPermission(VayuPermissions.TaskTypes.Default, L("Permission:TaskTypes"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Create, L("Permission:TaskTypes.Create"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Edit, L("Permission:TaskTypes.Edit"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Delete, L("Permission:TaskTypes.Delete"));

        var rolePermission = vayuGroup.AddPermission(VayuPermissions.Roles.Default, L("Permission:Roles"));
        taskTypePermission.AddChild(VayuPermissions.Roles.Create, L("Permission:Roles.Create"));
        taskTypePermission.AddChild(VayuPermissions.Roles.Edit, L("Permission:Roles.Edit"));
        taskTypePermission.AddChild(VayuPermissions.Roles.Delete, L("Permission:Roles.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VayuResource>(name);
    }
}
