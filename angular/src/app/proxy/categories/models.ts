import type { AuditedEntityDto } from '@abp/ng.core';

export interface CategoryDto extends AuditedEntityDto<string> {
  parentId?: string;
  name?: string;
  description?: string;
  parentCategoryId?: string;
  parentCategory: CategoryDto;
  subCategories: CategoryDto[];
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug?: string;
}

export interface CreateUpdateCategoryDto {
  parentId: string;
  name: string;
  description?: string;
  parentCategoryId?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug?: string;
}
