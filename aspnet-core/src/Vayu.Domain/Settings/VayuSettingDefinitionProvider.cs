using Volo.Abp.Settings;

namespace Vayu.Settings;

public class VayuSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(VayuSettings.MySetting1));
    }
}
