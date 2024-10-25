using Microsoft.Extensions.Localization;
using WebMaker.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace WebMaker;

[Dependency(ReplaceServices = true)]
public class WebMakerBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WebMakerResource> _localizer;

    public WebMakerBrandingProvider(IStringLocalizer<WebMakerResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
