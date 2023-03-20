import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { TaskCategory } from './task-category.enum';

export interface TaskTypeCreateUpdateDto extends EntityDto<string> {
  name: string;
  taskCategory: TaskCategory;
}

export interface TaskTypeDto extends FullAuditedEntityDto<string> {
  name?: string;
  taskCategory: TaskCategory;
}

export interface TaskTypeFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  taskCategory?: TaskCategory;
}
