using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using WebMaker.Categories;

namespace WebMaker;

[DependsOn(
    typeof(WebMakerDomainModule),
    typeof(WebMakerApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
[DependsOn(typeof(AbpAutoMapperModule))]
public class WebMakerApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WebMakerApplicationModule>();
        });
        
        Configure<AbpExceptionHandlingOptions>(options =>
        {
            options.SendExceptionsDetailsToClients = true;
            options.SendStackTraceToClients = true;
        });
        
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WebMakerDomainModule>();
        });
        context.Services.AddTransient<ICategoryAppService, CategoryAppService>();

    }
}
