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
            this._categoryRepository = categoryRepository;
        }

        public async Task<PagedResultDto<CategoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _categoryRepository.CountAsync();

            var categories = await _categoryRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting
            );

            List<CategoryDto> categoryDtos = ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);

            return new PagedResultDto<CategoryDto>(totalCount, categoryDtos);
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateUpdateCategoryDto input)
        {
            // AutoMapper kullanarak DTO'dan entity'ye dönüşüm
            var category = ObjectMapper.Map<CreateUpdateCategoryDto, Category>(input);

            category = await _categoryRepository.InsertAsync(category, autoSave: true);

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdateCategoryDto input)
        {
            var category = await _categoryRepository.GetAsync(id);

            // AutoMapper kullanarak DTO'dan entity'ye güncelleme
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