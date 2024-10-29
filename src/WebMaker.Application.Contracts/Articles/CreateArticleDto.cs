using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMaker.Articles
{
    public class CreateArticleDto
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

        [Required]
        public ArticleType type { get; set; }

    }
}
