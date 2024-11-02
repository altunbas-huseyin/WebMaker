using System;
using System.ComponentModel.DataAnnotations;

namespace WebMaker.Categories;

public class CreateUpdateCategoryDto
{
    [Required]
    [StringLength(CategoryConsts.MaxNameLength)]
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string SeoTitle { get; set; }
    public string SeoDescription { get; set; }
    public string SeoKeywords { get; set; }
    public string SeoSlug { get; set; }
}