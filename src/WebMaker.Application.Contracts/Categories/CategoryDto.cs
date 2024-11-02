using System;
using System.Collections.Generic;

namespace WebMaker.Categories;

public class CategoryDto
{
    public Guid Id { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string SeoSlug { get; set; }
    public string LanguageCode { get; set; }
    public List<CategoryTranslationDto> Translations { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
}