using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WebMaker.Categories;

namespace WebMaker.Repositories;

public interface ICategoryTranslationRepository:IRepository<CategoryTranslation, Guid>
{
    Task<bool> Test(string slug, Guid? excludeCategoryId = null);

}