import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { TaskTypeEnum } from './task-type-enum.enum';

export interface TaskTypeCreateUpdateDto extends EntityDto<string> {
  name: string;
  type: TaskTypeEnum;
}

export interface TaskTypeDto extends FullAuditedEntityDto<string> {
  name?: string;
  type: TaskTypeEnum;
}

export interface TaskTypeFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  type: TaskTypeEnum;
}
