import {AfterViewInit, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Chart} from "chart.js/auto";
import {DatePipe} from "@angular/common";

@Component({
  selector: 'app-public-chart',
  templateUrl: './public-chart.component.html',
  styleUrls: ['./public-chart.component.scss']
})
export class PublicChartComponent implements OnInit, AfterViewInit {
  @ViewChild('doughnutCanvas') doughnutCanvas: ElementRef | undefined;
  doughnutChart: any;
  monthOptions = [] as number[];

  constructor(private datePipe: DatePipe) {}

  ngOnInit(): void {
    const previous6Month = new Date();
    previous6Month.setMonth(previous6Month.getMonth() - 6);
    console.log(this.datePipe.transform(previous6Month,'M'))
    for (let i = (new Date().setMonth(-6)); i < (new Date().getMonth()); i++) {
      this.monthOptions.push(i);
    }
    console.log(this.monthOptions)
  }

  ngAfterViewInit() {
    this.doughnutChartMethod();
  }

  doughnutChartMethod() {
    this.doughnutChart = new Chart(this.doughnutCanvas?.nativeElement, {
      type: 'doughnut',
      data: {
        labels: ['BJP', 'Congress', 'AAP', 'CPM', 'SP'],
        datasets: [
          {
            label: '# of Votes',
            data: [50, 29, 15, 10, 7],
            backgroundColor: [
              'rgba(255, 159, 64, 0.2)',
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
            ],
            hoverBackgroundColor: [
              '#FFCE56',
              '#FF6384',
              '#36A2EB',
              '#FFCE56',
              '#FF6384',
            ],
          },
        ],
      },
      options: {
        aspectRatio:2
      }
    });
  }
}
