using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using WebMaker.Categories;

namespace WebMaker.Categories;

 public class Category : AuditedAggregateRoot<Guid>
    {
        public Guid ParentId { get; set; }
        public string SeoSlug { get; set; }

        // Hierarchical structure
        public Guid? ParentCategoryId { get; set; }
        
        [JsonIgnore]
        public virtual Category ParentCategory { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Category> SubCategories { get; set; }

        [JsonIgnore] 
        public virtual ICollection<CategoryTranslation> Translations { get; protected set; }
        
        [JsonIgnore]
        public virtual ICollection<ArticleCategory> Articles { get; set; }

        protected Category()
        {
            SubCategories = new HashSet<Category>();
            Articles = new HashSet<ArticleCategory>();
            Translations = new HashSet<CategoryTranslation>();
        }

        public Category(
            Guid id,
            string defaultName,
            string defaultLanguage,
            Guid? parentCategoryId = null)
            : base(id)
        {
            SubCategories = new HashSet<Category>();
            Articles = new HashSet<ArticleCategory>();
            Translations = new HashSet<CategoryTranslation>();
            ParentCategoryId = parentCategoryId;

            AddTranslation(defaultLanguage, defaultName);
        }

        public void AddTranslation(string languageCode, string name)
        {
            if (Translations.Any(x => x.LanguageCode == languageCode))
            {
                throw new BusinessException("Category:TranslationAlreadyExists");
            }

            Translations.Add(new CategoryTranslation(Id, languageCode, name));
        }

        public void RemoveTranslation(string languageCode)
        {
            if (Translations.Count == 1)
            {
                throw new BusinessException("Category:CannotRemoveLastTranslation");
            }

            Translations.RemoveAll(x => x.LanguageCode == languageCode);
        }

        public void UpdateTranslation(
            string languageCode,
            string name,
            string description = null,
            string seoTitle = null,
            string seoDescription = null,
            string seoKeywords = null)
        {
            var translation = Translations.FirstOrDefault(x => x.LanguageCode == languageCode);
            
            if (translation == null)
            {
                translation = new CategoryTranslation(Id, languageCode, name);
                Translations.Add(translation);
            }
            else
            {
                translation.Name = Check.NotNullOrWhiteSpace(name, nameof(name), CategoryConsts.MaxNameLength);
            }

            translation.Description = description;
            translation.SeoTitle = seoTitle;
            translation.SeoDescription = seoDescription;
            translation.SeoKeywords = seoKeywords;
        }

        public CategoryTranslation GetTranslation(string languageCode)
        {
            return Translations.FirstOrDefault(x => x.LanguageCode == languageCode);
        }

        public void SetSeoSlug(string seoSlug)
        {
            SeoSlug = Check.NotNullOrWhiteSpace(seoSlug, nameof(seoSlug), CategoryConsts.MaxSeoSlugLength);
        }
    }