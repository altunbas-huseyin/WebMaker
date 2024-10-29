using System;
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
       public ArticleType type { get; set; }
    }
}
