import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { PublicChartComponent } from './public-chart/public-chart.component';
import {MatButtonToggleModule} from "@angular/material/button-toggle";

@NgModule({
  declarations: [HomeComponent, PublicChartComponent],
  imports: [SharedModule, HomeRoutingModule, MatButtonToggleModule]
})
export class HomeModule {}
