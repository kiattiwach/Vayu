import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { Status } from './status.enum';
import type { TaskTypeDto } from '../task-types/models';
import type { StaffDto } from '../staffs/models';

export interface TaskCreateUpdateDto extends EntityDto<string> {
  name: string;
  description: string;
  taskTypeId?: string;
  status: Status;
  createDateTime: string;
  completeDateTime?: string;
  note?: string;
  leaderId: string;
  contactPerson?: string;
  contactNumber?: string;
  teamMemberIds: string[];
}

export interface TaskDto extends FullAuditedEntityDto<string> {
  name?: string;
  description?: string;
  taskTypeId?: string;
  status: Status;
  createDateTime?: string;
  completeDateTime?: string;
  note?: string;
  leaderId?: string;
  contactPerson?: string;
  contactNumber?: string;
  concurrencyStamp?: string;
}

export interface TaskFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  name?: string;
  description?: string;
  taskTypeId?: string;
  status?: Status;
  minCreateDateTime?: string;
  maxCreateDateTime?: string;
  minCompleteDateTime?: string;
  maxCompleteDateTime?: string;
  note?: string;
  staffId?: string;
  contactPerson?: string;
  contactNumber?: string;
}

export interface TaskWithNavigatePropertiesDto {
  task: TaskDto;
  taskType: TaskTypeDto;
  staff: StaffDto;
  teamMembers: StaffDto[];
}
