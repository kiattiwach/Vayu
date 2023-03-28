import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskRoutingModule } from './task-routing.module';
import { TaskComponent } from './task.component';
import {CoreModule, LocalizationModule} from "@abp/ng.core";
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {BaseThemeSharedModule} from "@abp/ng.theme.shared";
import {ReactiveFormsModule} from "@angular/forms";
import {NgbDatepickerModule, NgbDropdownModule} from "@ng-bootstrap/ng-bootstrap";
import {AdvancedEntityFiltersModule, CommercialUiModule} from "@volo/abp.commercial.ng.ui";
import {MatTabsModule} from "@angular/material/tabs";
import {MatButtonModule} from "@angular/material/button";
import {MatListModule} from "@angular/material/list";
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";


@NgModule({
  declarations: [
    TaskComponent
  ],
  imports: [
    CommonModule,
    TaskRoutingModule,
    LocalizationModule,
    NgxDatatableModule,
    BaseThemeSharedModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    CoreModule,
    CommercialUiModule,
    AdvancedEntityFiltersModule,
    NgbDatepickerModule,
    MatTabsModule,
    MatButtonModule,
    MatListModule,
    MatInputModule,
    MatSelectModule
  ]
})
export class TaskModule { }
