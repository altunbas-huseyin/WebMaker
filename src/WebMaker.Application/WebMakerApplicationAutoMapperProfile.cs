using AutoMapper;
using WebMaker.Categories;

namespace WebMaker;

public class WebMakerApplicationAutoMapperProfile : Profile
{
    public WebMakerApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        
        CreateMap<Category, CreateUpdateCategoryDto>();
        CreateMap<CreateUpdateCategoryDto, Category>(); // Yeni eklenen satır

    }
}
