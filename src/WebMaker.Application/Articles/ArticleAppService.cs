using System;
using System.Threading.Tasks;
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
         UpdateArticleDto>, IArticleAppService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleAppService(IArticleRepository repository)
            : base(repository)
        {
            _articleRepository = repository;
        }

        public override async Task<ArticleDto> CreateAsync(CreateArticleDto input)
        {
            var article = new Article(
                GuidGenerator.Create(),
                input.Title,
                input.Content,
                input.Summary,
                input.type
            );

            await Repository.InsertAsync(article);
            return await MapToGetOutputDtoAsync(article);
        }

        public override async Task<ArticleDto> UpdateAsync(Guid id, UpdateArticleDto input)
        {
            var article = await Repository.GetAsync(id);

            article.SetTitle(input.Title);
            article.SetContent(input.Content);
            article.SetSummary(input.Summary);

            await Repository.UpdateAsync(article);
            return await MapToGetOutputDtoAsync(article);
        }

        public async Task PublishAsync(Guid id)
        {
            var article = await Repository.GetAsync(id);
            article.Publish();
            await Repository.UpdateAsync(article);
        }

        public async Task UnpublishAsync(Guid id)
        {
            var article = await Repository.GetAsync(id);
            article.UnPublish();
            await Repository.UpdateAsync(article);
        }
    }
}
