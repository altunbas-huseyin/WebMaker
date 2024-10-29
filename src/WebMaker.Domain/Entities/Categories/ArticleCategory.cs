using System;
using Volo.Abp.Domain.Entities;
using WebMaker.Entities.Articles;

namespace WebMaker.Categories
{
    public class ArticleCategory : Entity
    {
        public Guid ArticleId { get; protected set; }
        public Guid CategoryId { get; protected set; }

        public virtual Article Article { get; protected set; }
        public virtual Category Category { get; protected set; }

        protected ArticleCategory()
        {
        }

        public ArticleCategory(Guid articleId, Guid categoryId)
        {
            ArticleId = articleId;
            CategoryId = categoryId;
        }

        public override object[] GetKeys()
        {
            return new object[] { ArticleId, CategoryId };
        }
    }
}
