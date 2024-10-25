using WebMaker.Samples;
using Xunit;

namespace WebMaker.EntityFrameworkCore.Domains;

[Collection(WebMakerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<WebMakerEntityFrameworkCoreTestModule>
{

}
