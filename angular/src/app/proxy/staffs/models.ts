import type { AuditedEntityDto, EntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { RoleDto } from '../roles/models';

export interface StaffCreateUpdateDto extends EntityDto<string> {
  code: string;
  name: string;
  surname: string;
  codeName: string;
  contactNumber: string;
  startDate: string;
  passAssessmentDate?: string;
  email: string;
  address?: string;
  radioCode?: string;
  contactPerson?: string;
  contactPersonNumber?: string;
  line?: string;
  facebook?: string;
  roleId: string;
  userId?: string;
}

export interface StaffDto extends AuditedEntityDto<string> {
  code?: string;
  name?: string;
  surname?: string;
  codeName?: string;
  contactNumber?: string;
  startDate?: string;
  passAssessmentDate?: string;
  email?: string;
  address?: string;
  radioCode?: string;
  contactPerson?: string;
  contactPersonNumber?: string;
  line?: string;
  facebook?: string;
  roleId?: string;
}

export interface StaffFilterDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  code?: string;
  name?: string;
  surname?: string;
  codeName?: string;
  email?: string;
  address?: string;
  radioCode?: string;
  contactNumber?: string;
  contactPerson?: string;
  contactPersonNumber?: string;
  line?: string;
  facebook?: string;
  roleId?: string;
  minStartDate?: string;
  maxStartDate?: string;
  minPassAssessmentDate?: string;
  maxPassAssessmentDate?: string;
}

export interface StaffWithNavigatePropertiesDto {
  staff: StaffDto;
  role: RoleDto;
}
