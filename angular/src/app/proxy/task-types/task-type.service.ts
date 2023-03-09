import type { TaskTypeCreateUpdateDto, TaskTypeDto, TaskTypeFilterDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TaskTypeService {
  apiName = 'Vayu';
  

  create = (input: TaskTypeCreateUpdateDto) =>
    this.restService.request<any, TaskTypeDto>({
      method: 'GET',
      url: `/api/task-type/create/${input}`,
      params: { name: input.name, type: input.type, id: input.id },
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
      method: 'GET',
      url: `/api/task-type/get-list/${input}`,
      params: { filter: input.filter, type: input.type, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });
  

  update = (input: TaskTypeCreateUpdateDto) =>
    this.restService.request<any, TaskTypeDto>({
      method: 'GET',
      url: `/api/task-type/update/${input}`,
      params: { name: input.name, type: input.type, id: input.id },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
