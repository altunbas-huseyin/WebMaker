import type { ArticleDto, CreateArticleDto, UpdateArticleDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ArticleService {
  apiName = 'Default';
  

  create = (input: CreateArticleDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ArticleDto>({
      method: 'POST',
      url: '/api/app/article',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/article/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ArticleDto>({
      method: 'GET',
      url: `/api/app/article/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ArticleDto>>({
      method: 'GET',
      url: '/api/app/article',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  publish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/article/${id}/publish`,
    },
    { apiName: this.apiName,...config });
  

  unpublish = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/article/${id}/unpublish`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateArticleDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ArticleDto>({
      method: 'PUT',
      url: `/api/app/article/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
