using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMaker.Entities.Articles;

namespace WebMaker.Articles
{
    public class ArticleAppService : CrudAppService<
       Article,
       ArticleDto,
       Guid,
       PagedAndSortedResultRequestDto,
       CreateArticleDto,
       UpdateArticleDto>,
       IArticleAppService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleAppService(IArticleRepository repository)
            : base(repository)
        {
            _articleRepository = repository;
        }

        public override async Task<ArticleDto> CreateAsync(CreateArticleDto input)
        {
            await ValidateSlugAsync(input.SeoSlug);

            var article = new Article(
                GuidGenerator.Create(),
                input.Title,
                input.Content,
                input.Summary,
                input.Type,
                input.SeoTitle,
                input.SeoSlug
            );

            // Set full SEO data if provided
            if (!string.IsNullOrWhiteSpace(input.SeoDescription) ||
                !string.IsNullOrWhiteSpace(input.SeoKeywords))
            {
                article.SetFullSeoData(
                    input.SeoTitle,
                    input.SeoDescription,
                    input.SeoKeywords,
                    input.SeoSlug
                );
            }

            // Add categories if provided
            if (input.CategoryIds != null && input.CategoryIds.Count > 0)
            {
                foreach (var categoryId in input.CategoryIds)
                {
                    article.AddCategory(categoryId);
                }
            }

            await Repository.InsertAsync(article);
            return await MapToGetOutputDtoAsync(article);
        }

        public override async Task<ArticleDto> UpdateAsync(Guid id, UpdateArticleDto input)
        {
            var article = await Repository.GetAsync(id);

            // Validate slug if it's different from current
            if (article.SeoSlug != input.SeoSlug)
            {
                await ValidateSlugAsync(input.SeoSlug);
            }

            // Update basic properties
            article.SetTitle(input.Title);
            article.SetContent(input.Content);
            article.SetSummary(input.Summary);
            article.SetType(input.Type);

            // Update SEO data
            article.SetFullSeoData(
                input.SeoTitle,
                input.SeoDescription,
                input.SeoKeywords,
                input.SeoSlug
            );

            // Update categories
            if (input.CategoryIds != null)
            {
                article.UpdateCategories(input.CategoryIds);
            }

            await Repository.UpdateAsync(article);
            return await MapToGetOutputDtoAsync(article);
        }

        public async Task PublishAsync(Guid id)
        {
            var article = await Repository.GetAsync(id);

            try
            {
                article.Publish();
                await Repository.UpdateAsync(article);
            }
            catch (BusinessException ex)
            {
                throw new UserFriendlyException(
                    "Could not publish the article",
                    ex.Code,
                    ex.Details
                );
            }
        }

        public async Task UnpublishAsync(Guid id)
        {
            var article = await Repository.GetAsync(id);
            article.UnPublish();
            await Repository.UpdateAsync(article);
        }

        private async Task ValidateSlugAsync(string slug)
        {
            // Check if slug is already in use
            var existingArticle = await _articleRepository.FindBySlugAsync(slug);
            if (existingArticle != null)
            {
                throw new UserFriendlyException(
                    "Invalid slug",
                    "Article:DuplicateSlug",
                    "An article with this slug already exists. Please choose a different slug."
                );
            }
        }

        protected override IQueryable<Article> ApplyDefaultSorting(IQueryable<Article> query)
        {
            return query.OrderByDescending(x => x.CreationTime);
        }
    }

    // Interface extension
    public interface IArticleAppService :
        ICrudAppService<
            ArticleDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateArticleDto,
            UpdateArticleDto>
    {
        Task PublishAsync(Guid id);
        Task UnpublishAsync(Guid id);
    }
}
