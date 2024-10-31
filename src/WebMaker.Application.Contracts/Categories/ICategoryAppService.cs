using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMaker.Categories
{
    public interface ICategoryAppService : IApplicationService, ICrudAppService<
        CategoryDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDto>
    {
        Task<List<CategoryDto>> GetAllWithChildrenAsync();
        Task<CategoryDto> GetWithChildrenAsync(Guid id);
        Task<List<CategoryDto>> GetChildrenAsync(Guid? parentId);
    }
}
