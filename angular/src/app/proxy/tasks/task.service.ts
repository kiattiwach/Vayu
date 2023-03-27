import type { TaskCreateUpdateDto, TaskDto, TaskFilterDto, TaskWithNavigatePropertiesDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LookupDto, LookupRequestDto } from '../shared/models';

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

  constructor(private restService: RestService) {}
}
