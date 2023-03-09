import { mapEnumToOptions } from '@abp/ng.core';

export enum TaskTypeEnum {
  Internal = 0,
  External = 1,
}

export const taskTypeEnumOptions = mapEnumToOptions(TaskTypeEnum);
