import { mapEnumToOptions } from '@abp/ng.core';

export enum ArticleType {
  Standard = 1,
  News = 2,
  Blog = 3,
  Tutorial = 4,
  Review = 5,
}

export const articleTypeOptions = mapEnumToOptions(ArticleType);
