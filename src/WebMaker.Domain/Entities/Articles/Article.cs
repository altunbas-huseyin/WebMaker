using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using WebMaker.Articles;
using WebMaker.Categories;

namespace WebMaker.Entities.Articles
{
    public class Article : AuditedAggregateRoot<Guid>
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Summary { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime? PublishDate { get; private set; }

        // Article Type
        public ArticleType Type { get; private set; }

        // SEO Properties
        public string SeoTitle { get; private set; }
        public string SeoDescription { get; private set; }
        public string SeoKeywords { get; private set; }
        public string SeoSlug { get; private set; }

        // Categories
        public virtual ICollection<ArticleCategory> Categories { get; private set; }

        protected Article()
        {
            Categories = new HashSet<ArticleCategory>();
        }

        public Article(
            Guid id,
            string title,
            string content,
            string summary,
            ArticleType type)
            : base(id)
        {
            SetTitle(title);
            SetContent(content);
            SetSummary(summary);
            SetType(type);
            IsPublished = false;
            Categories = new HashSet<ArticleCategory>();
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ArticleConsts.MaxTitleLength
            );
        }

        public void SetContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(
                content,
                nameof(content),
                maxLength: ArticleConsts.MaxContentLength
            );
        }

        public void SetSummary(string summary)
        {
            Summary = Check.NotNullOrWhiteSpace(
                summary,
                nameof(summary),
                maxLength: ArticleConsts.MaxSummaryLength
            );
        }

        public void SetType(ArticleType type)
        {
            Type = type;
        }

        public void SetSeoData(
            string seoTitle,
            string seoDescription,
            string seoKeywords,
            string seoSlug)
        {
            SeoTitle = Check.NotNullOrWhiteSpace(
                seoTitle,
                nameof(seoTitle),
                maxLength: ArticleConsts.MaxSeoTitleLength
            );

            SeoDescription = Check.Length(
                seoDescription,
                nameof(seoDescription),
                maxLength: ArticleConsts.MaxSeoDescriptionLength
            );

            SeoKeywords = Check.Length(
                seoKeywords,
                nameof(seoKeywords),
                maxLength: ArticleConsts.MaxSeoKeywordsLength
            );

            SeoSlug = Check.NotNullOrWhiteSpace(
                seoSlug,
                nameof(seoSlug),
                maxLength: ArticleConsts.MaxSeoSlugLength
            );
        }

        public void AddCategory(Guid categoryId)
        {
            if (Categories.Any(x => x.CategoryId == categoryId))
            {
                return;
            }

            Categories.Add(new ArticleCategory(Id, categoryId));
        }

        public void RemoveCategory(Guid categoryId)
        {
            Categories.RemoveAll(x => x.CategoryId == categoryId);
        }

        public void ClearCategories()
        {
            Categories.Clear();
        }

        public void Publish()
        {
            if (IsPublished)
            {
                return;
            }

            IsPublished = true;
            PublishDate = DateTime.Now;
        }

        public void UnPublish()
        {
            if (!IsPublished)
            {
                return;
            }

            IsPublished = false;
            PublishDate = null;
        }

        public void UpdateCategories(IEnumerable<Guid> categoryIds)
        {
            Check.NotNull(categoryIds, nameof(categoryIds));

            // Remove old categories that are not in the new list
            Categories.RemoveAll(x => !categoryIds.Contains(x.CategoryId));

            // Add new categories
            foreach (var categoryId in categoryIds)
            {
                AddCategory(categoryId);
            }
        }
    }


}
