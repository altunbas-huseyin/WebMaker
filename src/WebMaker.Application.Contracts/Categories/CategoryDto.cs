using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace WebMaker.Categories
{
    public class CategoryDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public CategoryDto ParentCategory { get; set; }
        public List<CategoryDto> SubCategories { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoSlug { get; set; }
    }
}
