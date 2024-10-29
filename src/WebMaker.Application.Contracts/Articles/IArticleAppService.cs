using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMaker.Articles
{
    public interface IArticleAppService : ICrudAppService<
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
