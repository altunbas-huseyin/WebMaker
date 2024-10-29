using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMaker.Entities.Articles;
using WebMaker.EntityFrameworkCore;

namespace WebMaker.Articles
{
    public class ArticleRepository : EfCoreRepository<WebMakerDbContext, Article, Guid>, IArticleRepository
    {
        public ArticleRepository(IDbContextProvider<WebMakerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Article> FindByTitleAsync(string title)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Set<Article>()
                .FirstOrDefaultAsync(article => article.Title == title);
        }
    }
}
