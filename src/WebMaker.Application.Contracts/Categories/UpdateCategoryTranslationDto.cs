using System.ComponentModel.DataAnnotations;

namespace WebMaker.Categories;

public class UpdateCategoryTranslationDto
{
    [Required]
    [StringLength(CategoryConsts.MaxNameLength)]
    public string Name { get; set; }

    public string Description { get; set; }
    public string SeoTitle { get; set; }
    public string SeoDescription { get; set; }
    public string SeoKeywords { get; set; }
}