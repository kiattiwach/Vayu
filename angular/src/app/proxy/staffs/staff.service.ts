import type { StaffCreateUpdateDto, StaffDto, StaffFilterDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { LookupDto, LookupRequestDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class StaffService {
  apiName = 'Vayu';
  

  create = (input: StaffCreateUpdateDto) =>
    this.restService.request<any, StaffDto>({
      method: 'POST',
      url: `/api/staff/create/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'GET',
      url: `/api/staff/delete/${id}`,
    },
    { apiName: this.apiName });
  

  get = (id: string) =>
    this.restService.request<any, StaffDto>({
      method: 'GET',
      url: `/api/staff/get/${id}`,
    },
    { apiName: this.apiName });
  

  getList = (input: StaffFilterDto) =>
    this.restService.request<any, PagedResultDto<StaffDto>>({
      method: 'POST',
      url: `/api/staff/get-list/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  getRoleLookup = (input: LookupRequestDto) =>
    this.restService.request<any, PagedResultDto<LookupDto<string>>>({
      method: 'POST',
      url: `/api/staff/get-role-lookup/${input}`,
      body: input,
    },
    { apiName: this.apiName });
  

  update = (input: StaffCreateUpdateDto) =>
    this.restService.request<any, StaffDto>({
      method: 'POST',
      url: `/api/staff/update/${input}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
