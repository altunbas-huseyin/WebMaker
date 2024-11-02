using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMaker.Categories;
using WebMaker.EntityFrameworkCore;

namespace WebMaker.Repositories;

   public class CategoryRepository :  EfCoreRepository<WebMakerDbContext, Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(IDbContextProvider<WebMakerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Category> FindByTitleAsync(string title)
        {
            return await (await GetQueryableAsync())
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Translations.Any(t => t.Name == title));
        }

        public async Task<Category> FindBySlugAsync(string slug)
        {
            return await (await GetQueryableAsync())
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.SeoSlug == slug);
        }

        public async Task<List<Category>> GetPublishedCategoriesAsync(
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null,
            bool includeTranslations = false)
        {
            var query = await GetQueryableAsync();

            if (includeTranslations)
            {
                query = query.Include(x => x.Translations);
            }

            if (!string.IsNullOrWhiteSpace(sorting))
            {
                // Implement your sorting logic here
                // Example: sorting by name
                query = query.OrderBy(x => x.Translations.FirstOrDefault().Name);
            }

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesByParentAsync(
            Guid? parentId,
            int skipCount = 0,
            int maxResultCount = 10)
        {
            var query = await GetQueryableAsync();

            query = query
                .Include(x => x.Translations)
                .Where(x => x.ParentCategoryId == parentId)
                .OrderBy(x => x.Translations.FirstOrDefault().Name);

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<int> GetCategoryCountByParentAsync(Guid? parentId)
        {
            return await (await GetQueryableAsync())
                .Where(x => x.ParentCategoryId == parentId)
                .CountAsync();
        }

        public async Task<List<Category>> GetAllWithTranslationsAsync()
        {
            return await (await GetQueryableAsync())
                .Include(x => x.Translations)
                .Include(x => x.ParentCategory)
                .Include(x => x.SubCategories)
                .OrderBy(x => x.Translations.FirstOrDefault().Name)
                .ToListAsync();
        }

        public async Task<Category> GetWithTranslationsAsync(Guid id)
        {
            return await (await GetQueryableAsync())
                .Include(x => x.Translations)
                .Include(x => x.ParentCategory)
                .Include(x => x.SubCategories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetListWithTranslationsAsync(
            string languageCode, 
            int skipCount = 0, 
            int maxResultCount = 10,
            string sorting = null)
        {
            var query = await GetQueryableAsync();

            // Her zaman Translations'ı include et
            query = query
                .Include(x => x.Translations)
                .Include(x => x.ParentCategory)
                .ThenInclude(x => x.Translations);

            // Sıralama varsa 
            if (!string.IsNullOrWhiteSpace(sorting))
            {
                // İlgili dildeki ada göre sırala
                query = query.OrderBy(x => x.Translations
                    .FirstOrDefault(t => t.LanguageCode == languageCode).Name);
            }
            else
            {
                // Varsayılan sıralama - dildeki ada göre 
                query = query.OrderBy(x => x.Translations
                    .FirstOrDefault().Name);
            }

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<List<Category>> GetListWithTranslationsAsync(
            string languageCode,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            string sorting = null,
            bool includeDetails = false)
        {
            var query = await GetQueryableAsync();

            // Her zaman Translations'ı include et
            query = query.Include(x => x.Translations);

            if (includeDetails)
            {
                query = query
                    .Include(x => x.ParentCategory)
                    .Include(x => x.SubCategories);
            }

            // Sıralama
            if (!string.IsNullOrWhiteSpace(sorting))
            {
                // İlgili dildeki isme göre sırala
                query = query.OrderBy(x => x.Translations
                    .FirstOrDefault(t => t.LanguageCode == languageCode).Name);
            }
            else
            {
                // Varsayılan sıralama
                query = query.OrderBy(x => x.Translations.FirstOrDefault().Name);
            }

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<bool> SlugExistsAsync(string slug, Guid? excludeCategoryId = null)
        {
            var query = await GetQueryableAsync();

            query = query.Where(x => x.SeoSlug == slug);

            if (excludeCategoryId.HasValue)
            {
                query = query.Where(x => x.Id != excludeCategoryId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<List<Category>> GetHierarchicalListAsync()
        {
            // Sadece kök kategorileri getir (ParentId null olanlar)
            var rootCategories = await (await GetQueryableAsync())
                .Include(x => x.Translations)
                .Include(x => x.SubCategories)
                    .ThenInclude(x => x.Translations)
                .Where(x => x.ParentCategoryId == null)
                .OrderBy(x => x.Translations.FirstOrDefault().Name)
                .ToListAsync();

            return rootCategories;
        }

        public async Task<Dictionary<DateTime, int>> GetPublishingStatisticsAsync(
            DateTime startDate,
            DateTime endDate)
        {
            var categories = await (await GetQueryableAsync())
                .Where(x => x.CreationTime >= startDate && x.CreationTime <= endDate)
                .Select(x => new { x.CreationTime })
                .ToListAsync();

            return categories
                .GroupBy(x => x.CreationTime.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );
        }
        
        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var category = await GetAsync(id, true, cancellationToken);
    
            if (category.SubCategories.Any())
            {
                throw new BusinessException("Category:CannotDeleteParentCategory");
            }
    
            await base.DeleteAsync(id, autoSave, cancellationToken);
        }
    }