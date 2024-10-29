using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMaker.Entities.Articles
{
    public interface IArticleRepository : IRepository<Article, Guid>
    {
        Task<Article> FindByTitleAsync(string title);
    }
}
