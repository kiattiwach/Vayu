import type { TaskCreateUpdateDto, TaskDto, TaskFilterDto, TaskStatusDto, TaskViewDto, TaskWithNavigatePropertiesDto } from './models';
import type { Status } from './status.enum';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LookupDto, LookupRequestDto } from '../shared/models';
import type { StaffDto } from '../staffs/models';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  apiName = 'Vayu';
  

  create = (input: TaskCreateUpdateDto) =>
    this.restService.request<any, TaskDto>({
      method: 'POST',
      url: `/api/task/create/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'GET',
      url: `/api/task/delete/${id}`,
    },
    { apiName: this.apiName });
  

  get = (id: string) =>
    this.restService.request<any, TaskDto>({
      method: 'GET',
      url: `/api/task/get/${id}`,
    },
    { apiName: this.apiName });
  

  getAvailableStaffs = (ids: string[]) =>
    this.restService.request<any, StaffDto[]>({
      method: 'POST',
      url: `/api/task/get-available-staffs/${ids}`,
      body: ids,
    },
    { apiName: this.apiName });
  

  getList = (input: TaskFilterDto) =>
    this.restService.request<any, PagedResultDto<TaskWithNavigatePropertiesDto>>({
      method: 'POST',
      url: `/api/task/get-list${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  getStaffLookup = (input: LookupRequestDto) =>
    this.restService.request<any, PagedResultDto<LookupDto<string>>>({
      method: 'POST',
      url: `/api/task/get-staff-lookup/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  getStaffMany = (ids: string[]) =>
    this.restService.request<any, StaffDto[]>({
      method: 'POST',
      url: `/api/task/get-staff-many/${ids}`,
      body: ids,
    },
    { apiName: this.apiName });
  

  getTaskInfo = (id: string) =>
    this.restService.request<any, TaskViewDto>({
      method: 'GET',
      url: `/api/task/get-task-info/${id}`,
    },
    { apiName: this.apiName });
  

  getTaskStatusList = (input: LookupRequestDto) =>
    this.restService.request<any, PagedResultDto<LookupDto<Status>>>({
      method: 'GET',
      url: '/api/task/get-task-status-list',
      params: { filter: input.filter, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  getTaskTypeLookup = (input: LookupRequestDto) =>
    this.restService.request<any, PagedResultDto<LookupDto<string>>>({
      method: 'POST',
      url: `/api/task/get-task-type-lookup/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  update = (input: TaskCreateUpdateDto) =>
    this.restService.request<any, TaskDto>({
      method: 'POST',
      url: `/api/task/update/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  updateTaskStatus = (input: TaskStatusDto) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/task/update-task-status/${input}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
