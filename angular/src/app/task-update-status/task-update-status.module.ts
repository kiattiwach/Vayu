import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskUpdateStatusRoutingModule } from './task-update-status-routing.module';
import { TaskUpdateStatusComponent } from './task-update-status.component';


@NgModule({
  declarations: [
    TaskUpdateStatusComponent
  ],
  imports: [
    CommonModule,
    TaskUpdateStatusRoutingModule
  ]
})
export class TaskUpdateStatusModule { }
