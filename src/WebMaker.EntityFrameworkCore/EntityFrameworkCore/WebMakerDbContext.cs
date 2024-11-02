using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using WebMaker.Entities.Articles;
using WebMaker.Categories;
using WebMaker.Articles;

namespace WebMaker.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class WebMakerDbContext :
    AbpDbContext<WebMakerDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }  

    #endregion

    public WebMakerDbContext(DbContextOptions<WebMakerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(WebMakerConsts.DbTablePrefix + "YourEntities", WebMakerConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});


        builder.Entity<Article>(b =>
        {
            b.ToTable(WebMakerConsts.DbTablePrefix + "Articles", WebMakerConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ArticleConsts.MaxTitleLength);

            b.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(ArticleConsts.MaxContentLength);

            b.Property(x => x.Summary)
                .IsRequired()
                .HasMaxLength(ArticleConsts.MaxSummaryLength);

            b.Property(x => x.SeoTitle)
                .HasMaxLength(ArticleConsts.MaxSeoTitleLength);

            b.Property(x => x.SeoDescription)
                .HasMaxLength(ArticleConsts.MaxSeoDescriptionLength);

            b.Property(x => x.SeoKeywords)
                .HasMaxLength(ArticleConsts.MaxSeoKeywordsLength);

            b.Property(x => x.SeoSlug)
                .HasMaxLength(ArticleConsts.MaxSeoSlugLength);

            // Index on SeoSlug
            b.HasIndex(x => x.SeoSlug);
        });

        

        builder.Entity<ArticleCategory>(b =>
        {
            b.ToTable(WebMakerConsts.DbTablePrefix + "ArticleCategories", WebMakerConsts.DbSchema);

            b.ConfigureByConvention();

            // Composite primary key
            b.HasKey(x => new { x.ArticleId, x.CategoryId });

            // Relationships
            b.HasOne(x => x.Article)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.ArticleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Category)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            b.HasIndex(x => x.ArticleId);
            b.HasIndex(x => x.CategoryId);
        });
        
          builder.Entity<Category>(b =>
        {
            b.ToTable(WebMakerConsts.DbTablePrefix + "Categories", WebMakerConsts.DbSchema);

            b.ConfigureByConvention();

            // Name ve Description gibi çevirilen alanlar CategoryTranslation'a taşındı
            b.Property(x => x.SeoSlug)
                .HasMaxLength(CategoryConsts.MaxSeoSlugLength);

            // Self-referencing relationship for hierarchical categories
            b.HasOne(x => x.ParentCategory)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.ParentCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Translations relationship
            b.HasMany(x => x.Translations)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Index on ParentCategoryId and SeoSlug
            b.HasIndex(x => x.ParentCategoryId);
            b.HasIndex(x => x.SeoSlug);
        });

        builder.Entity<CategoryTranslation>(b =>
        {
            b.ToTable(WebMakerConsts.DbTablePrefix + "CategoryTranslations", WebMakerConsts.DbSchema);

            // Composite primary key
            b.HasKey(x => new { x.CategoryId, x.LanguageCode });

            b.Property(x => x.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CategoryConsts.MaxNameLength);

            b.Property(x => x.Description)
                .HasMaxLength(CategoryConsts.MaxDescriptionLength);

            b.Property(x => x.SeoTitle)
                .HasMaxLength(CategoryConsts.MaxSeoTitleLength);

            b.Property(x => x.SeoDescription)
                .HasMaxLength(CategoryConsts.MaxSeoDescriptionLength);

            b.Property(x => x.SeoKeywords)
                .HasMaxLength(CategoryConsts.MaxSeoKeywordsLength);

            // Indexes
            b.HasIndex(x => x.CategoryId);
            b.HasIndex(x => x.LanguageCode);
            b.HasIndex(x => new { x.CategoryId, x.LanguageCode }).IsUnique();
        });
    }
}
