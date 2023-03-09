import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskTypeRoutingModule } from './task-type-routing.module';
import { TaskTypeComponent } from './task-type.component';


@NgModule({
  declarations: [
    TaskTypeComponent
  ],
  imports: [
    CommonModule,
    TaskTypeRoutingModule
  ]
})
export class TaskTypeModule { }
