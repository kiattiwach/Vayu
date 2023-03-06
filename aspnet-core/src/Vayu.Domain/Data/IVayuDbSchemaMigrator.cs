using System.Threading.Tasks;

namespace Vayu.Data;

public interface IVayuDbSchemaMigrator
{
    Task MigrateAsync();
}
