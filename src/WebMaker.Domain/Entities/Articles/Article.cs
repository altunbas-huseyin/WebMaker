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
        #region Properties
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Summary { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime? PublishDate { get; private set; }
        public ArticleType Type { get; private set; }

        // SEO Properties
        public string SeoTitle { get; private set; }
        public string SeoDescription { get; private set; }
        public string SeoKeywords { get; private set; }
        public string SeoSlug { get; private set; }

        // Categories
        public virtual ICollection<ArticleCategory> Categories { get; private set; }
        #endregion

        #region Constructors
        protected Article()
        {
            Categories = new HashSet<ArticleCategory>();
        }

        public Article(
            Guid id,
            string title,
            string content,
            string summary,
            ArticleType type,
            string seoTitle,
            string seoSlug) : base(id)
        {
            SetTitle(title);
            SetContent(content);
            SetSummary(summary);
            SetType(type);

            // Set required SEO properties
            SetBasicSeoData(seoTitle, seoSlug);

            IsPublished = false;
            Categories = new HashSet<ArticleCategory>();
        }
        #endregion

        #region Public Methods
        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ArticleConsts.MaxTitleLength
            );

            // Update SEO title if not explicitly set
            if (string.IsNullOrWhiteSpace(SeoTitle))
            {
                SeoTitle = Title;
            }
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

            // Update SEO description if not explicitly set
            if (string.IsNullOrWhiteSpace(SeoDescription))
            {
                SeoDescription = Summary;
            }
        }

        public void SetType(ArticleType type)
        {
            Type = type;
        }

        public void SetBasicSeoData(string seoTitle, string seoSlug)
        {
            SeoTitle = Check.NotNullOrWhiteSpace(
                seoTitle,
                nameof(seoTitle),
                maxLength: ArticleConsts.MaxSeoTitleLength
            );

            SeoSlug = Check.NotNullOrWhiteSpace(
                seoSlug,
                nameof(seoSlug),
                maxLength: ArticleConsts.MaxSeoSlugLength
            );
        }

        public void SetFullSeoData(
            string seoTitle,
            string seoDescription,
            string seoKeywords,
            string seoSlug)
        {
            SetBasicSeoData(seoTitle, seoSlug);

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
        }

        public void AddCategory(Guid categoryId)
        {
            Check.NotNull(categoryId, nameof(categoryId));

            if (Categories.Any(x => x.CategoryId == categoryId))
            {
                return;
            }

            Categories.Add(new ArticleCategory(Id, categoryId));
        }

        public void RemoveCategory(Guid categoryId)
        {
            Check.NotNull(categoryId, nameof(categoryId));
            Categories.RemoveAll(x => x.CategoryId == categoryId);
        }

        public void ClearCategories()
        {
            Categories.Clear();
        }

        public void UpdateCategories(IEnumerable<Guid> categoryIds)
        {
            Check.NotNull(categoryIds, nameof(categoryIds));

            // Remove categories that are not in the new list
            Categories.RemoveAll(x => !categoryIds.Contains(x.CategoryId));

            // Add new categories
            foreach (var categoryId in categoryIds)
            {
                AddCategory(categoryId);
            }
        }

        public void Publish()
        {
            if (IsPublished)
            {
                return;
            }

            ValidateBeforePublish();
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
        #endregion

        #region Private Methods
        private void ValidateBeforePublish()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new BusinessException("Article:TitleRequired");

            if (string.IsNullOrWhiteSpace(Content))
                throw new BusinessException("Article:ContentRequired");

            if (string.IsNullOrWhiteSpace(Summary))
                throw new BusinessException("Article:SummaryRequired");

            if (string.IsNullOrWhiteSpace(SeoTitle) || string.IsNullOrWhiteSpace(SeoSlug))
                throw new BusinessException("Article:BasicSeoDataRequired");
        }
        #endregion
    }

}
