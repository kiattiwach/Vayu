import { mapEnumToOptions } from '@abp/ng.core';

export enum TaskCategory {
  Internal = 1,
  External = 2,
}

export const taskCategoryOptions = mapEnumToOptions(TaskCategory);
