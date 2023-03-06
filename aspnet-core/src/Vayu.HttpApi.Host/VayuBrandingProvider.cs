using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Vayu;

[Dependency(ReplaceServices = true)]
public class VayuBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Vayu";
}
