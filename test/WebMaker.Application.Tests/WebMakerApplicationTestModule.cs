using Volo.Abp.Modularity;

namespace WebMaker;

[DependsOn(
    typeof(WebMakerApplicationModule),
    typeof(WebMakerDomainTestModule)
)]
public class WebMakerApplicationTestModule : AbpModule
{

}
