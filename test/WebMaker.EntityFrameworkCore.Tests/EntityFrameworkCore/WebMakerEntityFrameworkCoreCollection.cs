using Xunit;

namespace WebMaker.EntityFrameworkCore;

[CollectionDefinition(WebMakerTestConsts.CollectionDefinitionName)]
public class WebMakerEntityFrameworkCoreCollection : ICollectionFixture<WebMakerEntityFrameworkCoreFixture>
{

}
