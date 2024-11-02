using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebMaker.Repositories;

namespace WebMaker.Categories;

public class CategoryTranslationAppService : ApplicationService, ICategoryTranslationAppService  
{
    private readonly ICategoryTranslationRepository _categoryTranslationRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryTranslationAppService(
        ICategoryTranslationRepository categoryTranslationRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryTranslationRepository = categoryTranslationRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryTranslationDto> CreateAsync(CreateCategoryTranslationDto input)
    {
        var category = await _categoryRepository.FindAsync(input.CategoryId);
        if (category == null)
        {
            throw new BusinessException(
                code: "Category:NotFound",
                message: $"Category with id {input.CategoryId} not found!");
        }

        var existingTranslation = await _categoryTranslationRepository.FindAsync(
            x => x.CategoryId == input.CategoryId && x.LanguageCode == input.LanguageCode
        );

        if (existingTranslation != null)
        {
            throw new BusinessException(
                code: "CategoryTranslation:AlreadyExists",
                message: $"Translation already exists for category {input.CategoryId} and language {input.LanguageCode}"
            );
        }

        var translation = new CategoryTranslation(
            input.CategoryId,
            input.LanguageCode,
            input.Name
        );

        // Map remaining properties
        _mapper.Map(input, translation);

        await _categoryTranslationRepository.InsertAsync(translation);

        return _mapper.Map<CategoryTranslation, CategoryTranslationDto>(translation);
    }

    public async Task<CategoryTranslationDto> UpdateTranslationAsync(
        Guid id,
        string languageCode,
        UpdateCategoryTranslationDto input)
    {
        var translation = await _categoryTranslationRepository.FindAsync(
            x => x.CategoryId == id && x.LanguageCode == languageCode
        );

        if (translation == null)
        {
            throw new BusinessException(
                code: "CategoryTranslation:NotFound",
                message: $"Translation not found for category {id} and language {languageCode}"
            );
        }

        _mapper.Map(input, translation);

        await _categoryTranslationRepository.UpdateAsync(translation);

        return _mapper.Map<CategoryTranslation, CategoryTranslationDto>(translation);
    }

    public async Task DeleteTranslationAsync(Guid id, string languageCode)
    {
        var translation = await _categoryTranslationRepository.FindAsync(
            x => x.CategoryId == id && x.LanguageCode == languageCode
        );

        if (translation == null)
        {
            throw new BusinessException(
                code: "CategoryTranslation:NotFound",
                message: $"Translation not found for category {id} and language {languageCode}"
            );
        }

        var totalTranslations = await _categoryTranslationRepository.CountAsync(
            x => x.CategoryId == id
        );

        if (totalTranslations <= 1)
        {
            throw new BusinessException(
                code: "CategoryTranslation:CannotDeleteLastTranslation",
                message: "Cannot delete the last translation of a category"
            );
        }

        await _categoryTranslationRepository.DeleteAsync(translation);
    }

    public async Task<ListResultDto<CategoryTranslationDto>> GetAllAsync()
    {
        var translations = await _categoryTranslationRepository.GetListAsync(includeDetails: true);

        var dtos = _mapper.Map<List<CategoryTranslation>, List<CategoryTranslationDto>>(translations);
        return new ListResultDto<CategoryTranslationDto>(dtos);
    }
}