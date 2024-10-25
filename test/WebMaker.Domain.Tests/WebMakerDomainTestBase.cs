using Volo.Abp.Modularity;

namespace WebMaker;

/* Inherit from this class for your domain layer tests. */
public abstract class WebMakerDomainTestBase<TStartupModule> : WebMakerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
