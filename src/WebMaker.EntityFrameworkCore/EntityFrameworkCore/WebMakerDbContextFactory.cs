using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebMaker.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class WebMakerDbContextFactory : IDesignTimeDbContextFactory<WebMakerDbContext>
{
    public WebMakerDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        WebMakerEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<WebMakerDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new WebMakerDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../WebMaker.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
