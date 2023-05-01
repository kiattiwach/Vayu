import {AfterViewInit, Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild} from '@angular/core';
import {Chart} from "chart.js/auto";
import {DatePipe} from "@angular/common";
import {LocalizationService} from "@abp/ng.core";
import {PublicChartFilterDto, PublicChartService} from "@proxy/public-charts";
import {Subscription} from "rxjs";

export class ChartEntity {
  constructor(label: string, labels: string[], data:[]) {
    this.label = label;
    this.labels = labels;
    this.data = data;
  }
  label: string;
  labels: string[];
  data: [];
}

@Component({
  selector: 'app-public-chart',
  templateUrl: './public-chart.component.html',
  styleUrls: ['./public-chart.component.scss']
})
export class PublicChartComponent implements OnInit, AfterViewInit, OnDestroy {
  @Output() selectedMonthEmitter: EventEmitter<string>
    = new EventEmitter<string>(null);
  @ViewChild('publicChart') publicChart: ElementRef | undefined;

  subscription: Subscription;
  backwardMonth = 6;
  selectedMonth = {} as PublicChartFilterDto;
  monthOption = [] as PublicChartFilterDto[];
  chart: any;
  chatEntity: ChartEntity;

  constructor(private datePipe: DatePipe,
              private localizationService: LocalizationService,
              private publicChartService: PublicChartService) {}

  ngOnInit(): void {
    this.initMonthOption();

    //todo get chart entity from api
  }

  private initMonthOption() {
    const currentMonth = Number(this.datePipe.transform(new Date(), 'M')) - 1;
    let i = 0;
    do {
      const date = new Date().setMonth(currentMonth - i);
      this.monthOption.push({
        month: Number(this.datePipe.transform(date, 'M')),
        year: Number(this.datePipe.transform(date, 'YYYY','','th')) //todo -gat year from language code
      })
      i++;
    } while (i < this.backwardMonth)
    if (this.monthOption?.length > 0) {
      this.selectedMonth = this.monthOption[0];
      setTimeout(() => {
        this.emitMonth();
      }, 500);
    }
  }

  ngAfterViewInit() {
    this.doughnutChartMethod();
  }

  doughnutChartMethod() {
    this.chart = new Chart(this.publicChart?.nativeElement, {
      type: 'doughnut',
      data: {
        labels: [],
        datasets: [],
      },
      options: {
        aspectRatio: 1.5,
        responsive: true,
        /*
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Chart.js Doughnut Chart'
          }
        }*/
      }
    });
  }

  emitMonth() {
    this.subscription = this.publicChartService.getPublicChart(this.selectedMonth)
      .subscribe(x => {
        this.chart.data.labels = [...x.data.labels];
        this.chart.data.datasets = [...x.data.datasets];
        this.chart.options = x.options;
        this.chart.update();
      });
    this.localizationService.get('::Chart:Month:' + this.selectedMonth.month, this.selectedMonth.year.toString())
      .subscribe(x => this.selectedMonthEmitter.emit(x));
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
