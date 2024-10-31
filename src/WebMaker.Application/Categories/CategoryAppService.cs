using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using WebMaker.Entities.Categories;

namespace WebMaker.Categories
{
    [Authorize]
    public class CategoryAppService : ApplicationService, ICategoryAppService, ITransientDependency
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAppService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Get all categories including their children
        public async Task<List<CategoryDto>> GetAllWithChildrenAsync()
        {
            var categories = await _categoryRepository.GetAllWithChildrenAsync();
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);
        }

        // Get a specific category with its children
        public async Task<CategoryDto> GetWithChildrenAsync(Guid id)
        {
            var category = await _categoryRepository.GetWithChildrenAsync(id);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        // Get child categories for a specific parent
        public async Task<List<CategoryDto>> GetChildrenAsync(Guid? parentId)
        {
            var categories = await _categoryRepository.GetChildrenAsync(parentId);
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);
        }

        // Get a list of categories with paging
        public async Task<PagedResultDto<CategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _categoryRepository.CountAsync();

            var items = await _categoryRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting ?? nameof(Category.Name)
            );

            return new PagedResultDto<CategoryDto>(
                totalCount,
                ObjectMapper.Map<List<Category>, List<CategoryDto>>(items)
            );
        }

        // Get a specific category by ID
        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        // Create a new category
        public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
        {
            var category = ObjectMapper.Map<CreateUpdateCategoryDto, Category>(input);

            category = await _categoryRepository.InsertAsync(category, autoSave: true);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        // Update an existing category
        public async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
        {
            var category = await _categoryRepository.GetAsync(id);

            

            ObjectMapper.Map(input, category);
            await _categoryRepository.UpdateAsync(category, autoSave: true);

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        // Delete a category
        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetWithChildrenAsync(id);
          

            await _categoryRepository.DeleteAsync(id, autoSave: true);
        }
    }
}
