using System;
using System.Collections.Generic;
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

        [Required]
        public ArticleType Type { get; set; }

        // SEO Properties
        [Required]
        [StringLength(ArticleConsts.MaxSeoTitleLength)]
        public string SeoTitle { get; set; }

        [StringLength(ArticleConsts.MaxSeoDescriptionLength)]
        public string SeoDescription { get; set; }

        [StringLength(ArticleConsts.MaxSeoKeywordsLength)]
        public string SeoKeywords { get; set; }

        [Required]
        [StringLength(ArticleConsts.MaxSeoSlugLength)]
        public string SeoSlug { get; set; }

        // Categories
        public List<Guid> CategoryIds { get; set; }


    }
}
