
export interface CategoryDto {
  id?: string;
  parentCategoryId?: string;
  seoSlug?: string;
  translations: CategoryTranslationDto[];
  creationTime?: string;
  lastModificationTime?: string;
}

export interface CategoryTranslationDto {
  languageCode?: string;
  name?: string;
  description?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
}

export interface CreateUpdateCategoryDto {
  name: string;
  description?: string;
  parentCategoryId?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug?: string;
}

export interface UpdateCategoryTranslationDto {
  name: string;
  description?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
}
