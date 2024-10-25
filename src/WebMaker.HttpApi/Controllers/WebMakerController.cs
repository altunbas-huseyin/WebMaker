using WebMaker.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WebMaker.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WebMakerController : AbpControllerBase
{
    protected WebMakerController()
    {
        LocalizationResource = typeof(WebMakerResource);
    }
}
