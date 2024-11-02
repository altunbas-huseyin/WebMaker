using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMaker.Categories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input);
        Task<ListResultDto<CategoryDto>> GetAllAsync();
        Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input);
        Task DeleteAsync(Guid id);
        Task<CategoryDto> GetAsync(Guid id);
    }
}
