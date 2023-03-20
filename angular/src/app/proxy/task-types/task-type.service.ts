import type { TaskTypeCreateUpdateDto, TaskTypeDto, TaskTypeFilterDto } from './models';
import type { TaskCategory } from './task-category.enum';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LookupDto, LookupRequestDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class TaskTypeService {
  apiName = 'Vayu';
  

  create = (input: TaskTypeCreateUpdateDto) =>
    this.restService.request<any, TaskTypeDto>({
      method: 'POST',
      url: `/api/task-type/create/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'GET',
      url: `/api/task-type/delete/${id}`,
    },
    { apiName: this.apiName });
  

  get = (id: string) =>
    this.restService.request<any, TaskTypeDto>({
      method: 'GET',
      url: `/api/task-type/get/${id}`,
    },
    { apiName: this.apiName });
  

  getList = (input: TaskTypeFilterDto) =>
    this.restService.request<any, PagedResultDto<TaskTypeDto>>({
      method: 'POST',
      url: `/api/task-type/get-list/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  getTaskCategoryList = (input: LookupRequestDto) =>
    this.restService.request<any, PagedResultDto<LookupDto<TaskCategory>>>({
      method: 'GET',
      url: '/api/task-type/getTaskCategoryList',
      params: { filter: input.filter, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (input: TaskTypeCreateUpdateDto) =>
    this.restService.request<any, TaskTypeDto>({
      method: 'POST',
      url: `/api/task-type/update/${input}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
