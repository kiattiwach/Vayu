using Vayu.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Vayu;

[DependsOn(
    typeof(VayuEntityFrameworkCoreTestModule)
    )]
public class VayuDomainTestModule : AbpModule
{

}
