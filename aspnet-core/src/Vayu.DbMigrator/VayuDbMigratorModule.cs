using Vayu.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Vayu.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(VayuEntityFrameworkCoreModule),
    typeof(VayuApplicationContractsModule)
    )]
public class VayuDbMigratorModule : AbpModule
{

}
