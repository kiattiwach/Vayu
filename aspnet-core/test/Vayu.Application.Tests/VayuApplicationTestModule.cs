using Volo.Abp.Modularity;

namespace Vayu;

[DependsOn(
    typeof(VayuApplicationModule),
    typeof(VayuDomainTestModule)
    )]
public class VayuApplicationTestModule : AbpModule
{

}
