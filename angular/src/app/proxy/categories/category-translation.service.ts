import type { CategoryTranslationDto, CreateCategoryTranslationDto, UpdateCategoryTranslationDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CategoryTranslationService {
  apiName = 'Default';
  

  create = (input: CreateCategoryTranslationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CategoryTranslationDto>({
      method: 'POST',
      url: '/api/app/category-translation',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  deleteTranslation = (id: string, languageCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/category-translation/${id}/translation`,
      params: { languageCode },
    },
    { apiName: this.apiName,...config });
  

  getAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<CategoryTranslationDto>>({
      method: 'GET',
      url: '/api/app/category-translation',
    },
    { apiName: this.apiName,...config });
  

  updateTranslation = (id: string, languageCode: string, input: UpdateCategoryTranslationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CategoryTranslationDto>({
      method: 'PUT',
      url: `/api/app/category-translation/${id}/translation`,
      params: { languageCode },
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
