import type { Status } from './status.enum';
import type { AuditedEntityDto, EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { TaskTypeDto } from '../task-types/models';
import type { StaffDto } from '../staffs/models';

export interface AvailableStatusDto {
  status: Status;
  requiredNote: boolean;
}

export interface TaskCreateUpdateDto extends EntityDto<string> {
  taskTypeId: string;
  status: Status;
  description: string;
  createDateTime: string;
  completeDateTime?: string;
  note?: string;
  leaderId: string;
  contactPerson?: string;
  contactNumber?: string;
  teamMemberIds: string[];
}

export interface TaskDto extends FullAuditedEntityDto<string> {
  taskTypeId?: string;
  status: Status;
  description?: string;
  createDateTime?: string;
  completeDateTime?: string;
  note?: string;
  leaderId?: string;
  contactPerson?: string;
  contactNumber?: string;
  teamMemberIds: string[];
  concurrencyStamp?: string;
  overAllDuration?: string;
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

export interface TaskStatusDto extends AuditedEntityDto<string> {
  taskId?: string;
  status: Status;
  note?: string;
}

export interface TaskStatusTimelineDto {
  label?: string;
  icon?: string;
  url?: string;
  routerLink?: string;
  expanded?: boolean;
  disable?: boolean;
  visible?: boolean;
  active?: boolean;
  style: object;
  color?: string;
  styleClass?: string;
  title?: string;
  id?: string;
}

export interface TaskViewDto extends EntityDto<string> {
  taskType: TaskTypeDto;
  status?: Status;
  description?: string;
  createDateTime?: string;
  completeDateTime?: string;
  note?: string;
  leader: StaffDto;
  contactPerson?: string;
  contactNumber?: string;
  teamMembers: StaffDto[];
  statusTimeliness: TaskStatusTimelineDto[];
  latestStatusDuration?: string;
  overAllTimeSpan?: string;
  overAllDuration?: string;
  finalStatus?: Status;
}

export interface TaskWithNavigatePropertiesDto {
  task: TaskDto;
  taskType: TaskTypeDto;
  staff: StaffDto;
  teamMembers: StaffDto[];
  taskStatuses: TaskStatusDto[];
  availableStatus: AvailableStatusDto[];
  statusColor?: string;
  groupKey?: string;
}
