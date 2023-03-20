import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface RoleCreateUpdateDto extends EntityDto<string> {
  name: string;
  description?: string;
}

export interface RoleDto extends FullAuditedEntityDto<string> {
  name?: string;
  description?: string;
}

export interface RoleFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  name?: string;
  description?: string;
}
