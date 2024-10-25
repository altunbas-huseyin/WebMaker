using WebMaker.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace WebMaker.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WebMakerEntityFrameworkCoreModule),
    typeof(WebMakerApplicationContractsModule)
)]
public class WebMakerDbMigratorModule : AbpModule
{
}
