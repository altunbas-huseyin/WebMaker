
export interface CategoryDto {
  id?: string;
  parentCategoryId?: string;
  seoSlug?: string;
  languageCode?: string;
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

export interface CreateCategoryTranslationDto {
  categoryId: string;
  languageCode?: string;
  name: string;
  description?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
}

export interface CreateUpdateCategoryDto {
  name: string;
  languageCode?: string;
  description?: string;
  parentCategoryId?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
  seoSlug?: string;
}

export interface UpdateCategoryTranslationDto {
  categoryId: string;
  languageCode?: string;
  name: string;
  description?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
}
