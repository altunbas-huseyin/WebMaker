using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace WebMaker.Articles
{
    public class ArticleDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }
        public ArticleType Type { get; set; }

        // SEO Properties
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoSlug { get; set; }

        // Categories
        public List<Guid> CategoryIds { get; set; }


    }
}
