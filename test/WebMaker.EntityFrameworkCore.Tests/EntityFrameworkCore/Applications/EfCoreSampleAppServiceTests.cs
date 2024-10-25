using WebMaker.Samples;
using Xunit;

namespace WebMaker.EntityFrameworkCore.Applications;

[Collection(WebMakerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<WebMakerEntityFrameworkCoreTestModule>
{

}
