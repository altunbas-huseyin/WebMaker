using Volo.Abp.Modularity;

namespace WebMaker;

[DependsOn(
    typeof(WebMakerDomainModule),
    typeof(WebMakerTestBaseModule)
)]
public class WebMakerDomainTestModule : AbpModule
{

}
