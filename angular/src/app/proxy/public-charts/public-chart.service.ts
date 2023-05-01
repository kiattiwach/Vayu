import type { ChartDoughnutDto, PublicChartFilterDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PublicChartService {
  apiName = 'Vayu';
  

  getPublicChart = (input: PublicChartFilterDto) =>
    this.restService.request<any, ChartDoughnutDto>({
      method: 'POST',
      url: `/api/chart/get-public-chart/${input}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
