using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace WebMaker.Data;

/* This is used if database provider does't define
 * IWebMakerDbSchemaMigrator implementation.
 */
public class NullWebMakerDbSchemaMigrator : IWebMakerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
