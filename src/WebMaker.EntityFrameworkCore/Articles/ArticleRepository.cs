using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMaker.Entities.Articles;
using WebMaker.EntityFrameworkCore;
using System.Linq;

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

        public async Task<Article> FindBySlugAsync(string slug)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Set<Article>()
                .FirstOrDefaultAsync(article => article.SeoSlug == slug);
        }

        public async Task<List<Article>> GetPublishedArticlesAsync(
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null,
            bool includeCategories = false)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Set<Article>()
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.PublishDate);

            if (includeCategories)
            {
                query = (IOrderedQueryable<Article>)query.Include(x => x.Categories);
            }

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByCategoryAsync(
            Guid categoryId,
            bool onlyPublished = true,
            int skipCount = 0,
            int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Set<Article>()
                .Include(x => x.Categories)
                .Where(article => article.Categories
                    .Any(category => category.CategoryId == categoryId));

            if (onlyPublished)
            {
                query = query.Where(x => x.IsPublished);
            }

            return await query
                .OrderByDescending(x => x.PublishDate ?? x.CreationTime)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<int> GetArticleCountByCategoryAsync(
            Guid categoryId,
            bool onlyPublished = true)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Set<Article>()
                .Include(x => x.Categories)
                .Where(article => article.Categories
                    .Any(category => category.CategoryId == categoryId));

            if (onlyPublished)
            {
                query = query.Where(x => x.IsPublished);
            }

            return await query.CountAsync();
        }

        public async Task<List<Article>> GetAllWithCategoriesAsync()
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Set<Article>()
                .Include(x => x.Categories)
                .OrderByDescending(x => x.CreationTime)
                .ToListAsync();
        }

        public async Task<Article> GetWithCategoriesAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.Set<Article>()
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SlugExistsAsync(string slug, Guid? excludeArticleId = null)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.Set<Article>()
                .Where(x => x.SeoSlug == slug);

            if (excludeArticleId.HasValue)
            {
                query = query.Where(x => x.Id != excludeArticleId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Dictionary<DateTime, int>> GetPublishingStatisticsAsync(
            DateTime startDate,
            DateTime endDate)
        {
            var dbContext = await GetDbContextAsync();
            var publishedArticles = await dbContext.Set<Article>()
                .Where(x => x.IsPublished &&
                           x.PublishDate >= startDate &&
                           x.PublishDate <= endDate)
                .GroupBy(x => x.PublishDate.Value.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => x.Count);

            return publishedArticles;
        }

        public override async Task<IQueryable<Article>> WithDetailsAsync()
        {
            var query = await base.WithDetailsAsync();
            return query.Include(x => x.Categories);
        }
    }
}
