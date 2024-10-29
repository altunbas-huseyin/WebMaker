using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WebMaker.Categories;

namespace WebMaker.Entities.Categories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<List<Category>> GetAllWithChildrenAsync();
        Task<Category> GetWithChildrenAsync(Guid id);
        Task<List<Category>> GetChildrenAsync(Guid? parentId);
    }
}
