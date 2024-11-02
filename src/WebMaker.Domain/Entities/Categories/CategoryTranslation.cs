using System;
using System.Text.Json.Serialization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMaker.Categories;

public class CategoryTranslation : AuditedAggregateRoot<Guid>
{
    public Guid CategoryId { get; protected set; }
    public string LanguageCode { get; protected set; }
        
    public string Name { get; set; }
    public string Description { get; set; }
    public string SeoTitle { get; set; }
    public string SeoDescription { get; set; }
    public string SeoKeywords { get; set; }

    [JsonIgnore]
    public virtual Category Category { get; protected set; }

    protected CategoryTranslation()
    {
    }

    public CategoryTranslation(
        Guid categoryId,
        string languageCode,
        string name)
    {
        CategoryId = categoryId;
        LanguageCode = languageCode;
        Name = name;
    }

    public override object[] GetKeys()
    {
        return new object[] { CategoryId, LanguageCode };
    }
}