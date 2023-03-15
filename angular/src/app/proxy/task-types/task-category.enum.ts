import { mapEnumToOptions } from '@abp/ng.core';

export enum TaskCategory {
  Internal = 0,
  External = 1,
}

export const taskCategoryOptions = mapEnumToOptions(TaskCategory);
