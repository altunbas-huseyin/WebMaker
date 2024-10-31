using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMaker.Categories
{
    public class Category : AuditedAggregateRoot<Guid>
    {
        public Guid ParentId { get;  set; }
        public string Name { get;  set; }
        public string Description { get; set; }

        // Hierarchical structure
        public Guid? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }

        // SEO Properties
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoSlug { get; set; }

        public virtual ICollection<ArticleCategory> Articles { get;  set; }

        protected Category()
        {
            SubCategories = new HashSet<Category>();
            Articles = new HashSet<ArticleCategory>();
        }

        public Category(
            Guid id,
            string name,
            Guid? parentCategoryId = null)
            : base(id)
        {
            SetName(name);
            ParentCategoryId = parentCategoryId;
            SubCategories = new HashSet<Category>();
            Articles = new HashSet<ArticleCategory>();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CategoryConsts.MaxNameLength);
        }

        public void SetSeoData(string seoTitle, string seoDescription, string seoKeywords, string seoSlug)
        {
            SeoTitle = Check.NotNullOrWhiteSpace(seoTitle, nameof(seoTitle), CategoryConsts.MaxSeoTitleLength);
            SeoDescription = Check.Length(seoDescription, nameof(seoDescription), CategoryConsts.MaxSeoDescriptionLength);
            SeoKeywords = Check.Length(seoKeywords, nameof(seoKeywords), CategoryConsts.MaxSeoKeywordsLength);
            SeoSlug = Check.NotNullOrWhiteSpace(seoSlug, nameof(seoSlug), CategoryConsts.MaxSeoSlugLength);
        }
    }
}
