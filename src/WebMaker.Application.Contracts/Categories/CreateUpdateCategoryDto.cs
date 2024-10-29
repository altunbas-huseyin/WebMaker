using System;
using System.ComponentModel.DataAnnotations;

namespace WebMaker.Categories
{
    public class CreateUpdateCategoryDto
    {
        [Required]
        [StringLength(CategoryConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(CategoryConsts.MaxDescriptionLength)]
        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; }

        [StringLength(CategoryConsts.MaxSeoTitleLength)]
        public string SeoTitle { get; set; }

        [StringLength(CategoryConsts.MaxSeoDescriptionLength)]
        public string SeoDescription { get; set; }

        [StringLength(CategoryConsts.MaxSeoKeywordsLength)]
        public string SeoKeywords { get; set; }

        [StringLength(CategoryConsts.MaxSeoSlugLength)]
        public string SeoSlug { get; set; }
    }
}
