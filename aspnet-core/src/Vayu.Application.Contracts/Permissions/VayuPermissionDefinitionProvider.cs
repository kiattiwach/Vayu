using Vayu.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Vayu.Permissions;

public class VayuPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(VayuPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(VayuPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VayuResource>(name);
    }
}
