using System.Threading.Tasks;

namespace WebMaker.Data;

public interface IWebMakerDbSchemaMigrator
{
    Task MigrateAsync();
}
