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
        Task<CategoryDto> UpdateTranslationAsync(Guid id, string languageCode, UpdateCategoryTranslationDto input);
        Task DeleteTranslationAsync(Guid id, string languageCode);
        Task<ListResultDto<CategoryDto>> GetAllAsync();
    }
}
