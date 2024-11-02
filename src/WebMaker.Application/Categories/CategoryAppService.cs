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
using Volo.Abp;

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

    public async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
    {
        var category = await _categoryRepository.GetWithTranslationsAsync(id);
        
        if (category == null)
        {
            throw new UserFriendlyException("Category not found");
        }


        // Update translation
        category.UpdateTranslation(
            input.LanguageCode,
            input.Name,
            description: input.Description,
            seoTitle: input.SeoTitle,
            seoDescription: input.SeoDescription,
            seoKeywords: input.SeoKeywords
        );

        // Update parent category if provided
        if (input.ParentCategoryId.HasValue && input.ParentCategoryId.Value != category.ParentCategoryId)
        {
            // Check if parent category exists
            var parentCategory = await _categoryRepository.FindAsync(input.ParentCategoryId.Value);
            if (parentCategory == null)
            {
                throw new UserFriendlyException("Parent category not found");
            }
            category.ParentCategoryId = input.ParentCategoryId;
        }

        // Update SEO slug if provided
        if (!string.IsNullOrEmpty(input.SeoSlug))
        {
            // Check if slug is unique
            var slugExists = await _categoryRepository.SlugExistsAsync(input.SeoSlug, id);
            if (slugExists)
            {
                throw new UserFriendlyException("SEO slug already exists");
            }
            category.SetSeoSlug(input.SeoSlug);
        }

        await _categoryRepository.UpdateAsync(category);

        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetAsync(id);
        
        if (category == null)
        {
            throw new UserFriendlyException("Category not found");
        }

        // Check for sub-categories
        var hasSubCategories = await _categoryRepository.CountAsync(x => x.ParentCategoryId == id) > 0;
        if (hasSubCategories)
        {
            throw new UserFriendlyException("Cannot delete category with sub-categories");
        }

        await _categoryRepository.DeleteAsync(id);
    }

    public async Task<CategoryDto> GetAsync(Guid id)
    {
        var category = await _categoryRepository.GetWithTranslationsAsync(id);
        
        if (category == null)
        {
            throw new UserFriendlyException("Category not found");
        }

        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
    {
        var currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        
        // Check if SEO slug is unique if provided
        if (!string.IsNullOrEmpty(input.SeoSlug))
        {
            var slugExists = await _categoryRepository.SlugExistsAsync(input.SeoSlug);
            if (slugExists)
            {
                throw new UserFriendlyException("SEO slug already exists");
            }
        }

        // If parent category is provided, check if it exists
        if (input.ParentCategoryId.HasValue)
        {
            var parentExists = await _categoryRepository.FindAsync(input.ParentCategoryId.Value);
            if (parentExists == null)
            {
                throw new UserFriendlyException("Parent category not found");
            }
        }

        // Create new category
        var category = new Category(
            GuidGenerator.Create(),
            input.Name,
            currentLanguage,
            input.ParentCategoryId
        );

        // Add translation details if provided
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

        // Set SEO slug if provided
        if (!string.IsNullOrEmpty(input.SeoSlug))
        {
            category.SetSeoSlug(input.SeoSlug);
        }

        category = await _categoryRepository.InsertAsync(category, autoSave: true);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    
}