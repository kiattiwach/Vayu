import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskTypeRoutingModule } from './task-type-routing.module';
import { TaskTypeComponent } from './task-type.component';
import {LocalizationModule} from "@abp/ng.core";
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {BaseThemeSharedModule} from "@abp/ng.theme.shared";
import {ReactiveFormsModule} from "@angular/forms";
import {NgbDropdownModule} from "@ng-bootstrap/ng-bootstrap";


@NgModule({
  declarations: [
    TaskTypeComponent
  ],
    imports: [
        CommonModule,
        TaskTypeRoutingModule,
        LocalizationModule,
        NgxDatatableModule,
        BaseThemeSharedModule,
        ReactiveFormsModule,
        NgbDropdownModule
    ]
})
export class TaskTypeModule { }
