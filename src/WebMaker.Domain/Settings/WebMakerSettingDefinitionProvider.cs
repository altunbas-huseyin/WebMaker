using Volo.Abp.Settings;

namespace WebMaker.Settings;

public class WebMakerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WebMakerSettings.MySetting1));
    }
}
