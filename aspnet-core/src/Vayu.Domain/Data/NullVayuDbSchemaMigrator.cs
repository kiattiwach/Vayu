using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Vayu.Data;

/* This is used if database provider does't define
 * IVayuDbSchemaMigrator implementation.
 */
public class NullVayuDbSchemaMigrator : IVayuDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
