using Vayu.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Vayu.Permissions;

public class VayuPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var vayuGroup = context.AddGroup(VayuPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(VayuPermissions.MyPermission1, L("Permission:MyPermission1"));

        var taskTypePermission = vayuGroup.AddPermission(VayuPermissions.TaskTypes.Default, L("Permission:TaskTypes"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Create, L("Permission:TaskTypes.Create"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Edit, L("Permission:TaskTypes.Edit"));
        taskTypePermission.AddChild(VayuPermissions.TaskTypes.Delete, L("Permission:TaskTypes.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VayuResource>(name);
    }
}
