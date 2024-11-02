using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WebMaker.Categories;

public interface ICategoryTranslationAppService
{
    Task<CategoryTranslationDto> CreateAsync(CreateCategoryTranslationDto input);
    Task<CategoryTranslationDto> UpdateTranslationAsync(Guid id, string languageCode, UpdateCategoryTranslationDto input);
    Task DeleteTranslationAsync(Guid id, string languageCode);
    Task<ListResultDto<CategoryTranslationDto>> GetAllAsync();
}

