using Volo.Abp.Modularity;

namespace WebMaker;

public abstract class WebMakerApplicationTestBase<TStartupModule> : WebMakerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
