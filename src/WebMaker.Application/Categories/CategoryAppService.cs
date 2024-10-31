using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using System.Linq;

namespace WebMaker.Categories
{
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;

        public CategoryAppService(IRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedResultDto<CategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _categoryRepository.CountAsync();

            var categories = await _categoryRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting
            );

            var categoryDtos = ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);

            return new PagedResultDto<CategoryDto>(totalCount, categoryDtos);
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
        {
            var newCategory = new Category(
                id: GuidGenerator.Create(),
                name: input.Name,
                parentCategoryId: input.ParentCategoryId
            );

            if (!string.IsNullOrEmpty(input.Description))
            {
                newCategory.Description = input.Description;
            }

            if (!string.IsNullOrEmpty(input.SeoTitle))
            {
                newCategory.SetSeoData(
                    seoTitle: input.SeoTitle,
                    seoDescription: input.SeoDescription,
                    seoKeywords: input.SeoKeywords,
                    seoSlug: input.SeoSlug
                );
            }

            newCategory.ParentId = input.ParentId;

            newCategory = await _categoryRepository.InsertAsync(newCategory, autoSave: true);

            return ObjectMapper.Map<Category, CategoryDto>(newCategory);
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
        {
            var category = await _categoryRepository.GetAsync(id);

            ObjectMapper.Map(input, category);

            await _categoryRepository.UpdateAsync(category, autoSave: true);

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id, autoSave: true);
        }
    }
}