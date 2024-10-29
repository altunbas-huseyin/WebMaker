import type { AuditedEntityDto } from '@abp/ng.core';
import type { ArticleType } from './article-type.enum';

export interface ArticleDto extends AuditedEntityDto<string> {
  title?: string;
  content?: string;
  summary?: string;
  isPublished: boolean;
  publishDate?: string;
  type: ArticleType;
}

export interface CreateArticleDto {
  title: string;
  content: string;
  summary: string;
  type: ArticleType;
}

export interface UpdateArticleDto {
  title: string;
  content: string;
  summary: string;
}
