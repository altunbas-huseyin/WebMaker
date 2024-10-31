import type { AuditedEntityDto } from '@abp/ng.core';
import type { ArticleType } from './article-type.enum';

export interface ArticleDto extends AuditedEntityDto<string> {
  title?: string;
  content?: string;
  summary?: string;
  isPublished: boolean;
  publishDate?: string;
  type: ArticleType;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug?: string;
  categoryIds: string[];
}

export interface CreateArticleDto {
  title: string;
  content: string;
  summary: string;
  type: ArticleType;
  seoTitle: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug: string;
  categoryIds: string[];
}

export interface UpdateArticleDto {
  title: string;
  content: string;
  summary: string;
  type: ArticleType;
  seoTitle: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug: string;
  categoryIds: string[];
}
