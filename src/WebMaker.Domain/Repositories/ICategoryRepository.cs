using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WebMaker.Categories;

namespace WebMaker.Repositories;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<Category> GetWithTranslationsAsync(Guid id);
    Task<List<Category>> GetListWithTranslationsAsync(
        string languageCode,
        int skipCount = 0,
        int maxResultCount = 10,
        string sorting = null);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeCategoryId = null);
}