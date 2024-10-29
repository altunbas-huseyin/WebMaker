using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMaker.Entities.Articles
{
    public interface IArticleRepository : IRepository<Article, Guid>
    {
        Task<Article> FindByTitleAsync(string title);
        Task<Article> FindBySlugAsync(string slug);
        Task<List<Article>> GetPublishedArticlesAsync(
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null,
            bool includeCategories = false);
        Task<List<Article>> GetArticlesByCategoryAsync(
            Guid categoryId,
            bool onlyPublished = true,
            int skipCount = 0,
            int maxResultCount = 10);
        Task<int> GetArticleCountByCategoryAsync(
            Guid categoryId,
            bool onlyPublished = true);
        Task<List<Article>> GetAllWithCategoriesAsync();
        Task<Article> GetWithCategoriesAsync(Guid id);
        Task<bool> SlugExistsAsync(string slug, Guid? excludeArticleId = null);
        Task<Dictionary<DateTime, int>> GetPublishingStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}
