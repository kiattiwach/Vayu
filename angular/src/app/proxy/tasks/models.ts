import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { Status } from './status.enum';
import type { StaffDto } from '../staffs/models';
import type { TaskTypeDto } from '../task-types/models';

export interface TaskCreateUpdateDto extends EntityDto<string> {
  taskTypeId: string;
  status: Status;
  description: string;
  createDateTime: string;
  completeDateTime?: string;
  note?: string;
  leader: StaffDto;
  contactPerson?: string;
  contactNumber?: string;
  teamMemberIds: StaffDto[];
}

export interface TaskDto extends FullAuditedEntityDto<string> {
  taskTypeId?: string;
  status: Status;
  description?: string;
  createDateTime?: string;
  completeDateTime?: string;
  note?: string;
  leader: StaffDto;
  contactPerson?: string;
  contactNumber?: string;
  teamMembers: StaffDto[];
  concurrencyStamp?: string;
}

export interface TaskFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
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
