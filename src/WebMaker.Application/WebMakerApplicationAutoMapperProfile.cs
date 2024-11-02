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
        
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.Translations));

        CreateMap<CategoryTranslation, CategoryTranslationDto>()
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.SeoTitle, opt => opt.MapFrom(src => src.SeoTitle))
            .ForMember(dest => dest.SeoDescription, opt => opt.MapFrom(src => src.SeoDescription))
            .ForMember(dest => dest.SeoKeywords, opt => opt.MapFrom(src => src.SeoKeywords));

        CreateMap<CreateUpdateCategoryDto, Category>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Translations, opt => opt.Ignore())
            .ForMember(x => x.Articles, opt => opt.Ignore())
            .ForMember(x => x.SubCategories, opt => opt.Ignore())
            .ForMember(x => x.ParentCategory, opt => opt.Ignore());

        CreateMap<UpdateCategoryTranslationDto, CategoryTranslation>()
            .ForMember(x => x.CategoryId, opt => opt.Ignore())
            .ForMember(x => x.LanguageCode, opt => opt.Ignore())
            .ForMember(x => x.Category, opt => opt.Ignore());
    }
}
