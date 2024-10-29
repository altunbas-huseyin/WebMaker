using System.ComponentModel.DataAnnotations;

namespace WebMaker.Articles
{
    public class UpdateArticleDto
    {
        [Required]
        [StringLength(ArticleConsts.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ArticleConsts.MaxContentLength)]
        public string Content { get; set; }

        [Required]
        [StringLength(ArticleConsts.MaxSummaryLength)]
        public string Summary { get; set; }
    }
}
