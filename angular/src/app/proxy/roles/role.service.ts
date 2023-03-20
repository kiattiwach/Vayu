import type { RoleCreateUpdateDto, RoleDto, RoleFilterDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  apiName = 'Vayu';
  

  create = (input: RoleCreateUpdateDto) =>
    this.restService.request<any, RoleDto>({
      method: 'POST',
      url: `/api/role/create/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'GET',
      url: `/api/role/delete/${id}`,
    },
    { apiName: this.apiName });
  

  get = (id: string) =>
    this.restService.request<any, RoleDto>({
      method: 'GET',
      url: `/api/role/get/${id}`,
    },
    { apiName: this.apiName });
  

  getList = (input: RoleFilterDto) =>
    this.restService.request<any, PagedResultDto<RoleDto>>({
      method: 'POST',
      url: `/api/role/get-list/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  update = (input: RoleCreateUpdateDto) =>
    this.restService.request<any, RoleDto>({
      method: 'POST',
      url: `/api/role/update/${input}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
