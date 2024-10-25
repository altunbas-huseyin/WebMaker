using WebMaker.Localization;
using Volo.Abp.Application.Services;

namespace WebMaker;

/* Inherit your application services from this class.
 */
public abstract class WebMakerAppService : ApplicationService
{
    protected WebMakerAppService()
    {
        LocalizationResource = typeof(WebMakerResource);
    }
}
