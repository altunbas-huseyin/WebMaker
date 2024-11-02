using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using Microsoft.Extensions.Localization;
using WebMaker.Repositories;

namespace WebMaker.Categories;

public class CategoryAppService : ApplicationService, ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAppService(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<ListResultDto<CategoryDto>> GetAllAsync()
    {
        var currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    
        // Get categories with translations and other related data
        var categories = await _categoryRepository.GetListWithTranslationsAsync(
            languageCode: currentLanguage
        );

        // Map to DTOs using AutoMapper
        var categoryDtos = ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);

        return new ListResultDto<CategoryDto>(categoryDtos);
    }

    
    public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
    {
        var currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        
        var category = new Category(
            GuidGenerator.Create(),
            input.Name,
            currentLanguage,
            input.ParentCategoryId
        );

        if (!string.IsNullOrEmpty(input.Description))
        {
            category.UpdateTranslation(
                currentLanguage,
                input.Name,
                description: input.Description,
                seoTitle: input.SeoTitle,
                seoDescription: input.SeoDescription,
                seoKeywords: input.SeoKeywords
            );
        }

        if (!string.IsNullOrEmpty(input.SeoSlug))
        {
            category.SetSeoSlug(input.SeoSlug);
        }

        category = await _categoryRepository.InsertAsync(category, autoSave: true);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateTranslationAsync(
        Guid id,
        string languageCode,
        UpdateCategoryTranslationDto input)
    {
        var category = await _categoryRepository.GetWithTranslationsAsync(id);

        category.UpdateTranslation(
            languageCode,
            input.Name,
            input.Description,
            input.SeoTitle,
            input.SeoDescription,
            input.SeoKeywords
        );

        await _categoryRepository.UpdateAsync(category);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task DeleteTranslationAsync(Guid id, string languageCode)
    {
        var category = await _categoryRepository.GetWithTranslationsAsync(id);
        category.RemoveTranslation(languageCode);
        await _categoryRepository.UpdateAsync(category);
    }
}