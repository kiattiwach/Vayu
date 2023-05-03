import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskUpdateStatusRoutingModule } from './task-update-status-routing.module';
import { TaskUpdateStatusComponent } from './task-update-status.component';
import { CommercialUiModule} from "@volo/abp.commercial.ng.ui";
import {CoreModule, LocalizationModule} from "@abp/ng.core";
import {MatInputModule} from "@angular/material/input";
import {NgbDropdownModule} from "@ng-bootstrap/ng-bootstrap";
import {BaseThemeSharedModule} from "@abp/ng.theme.shared";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [
    TaskUpdateStatusComponent
  ],
  imports: [
    CommonModule,
    TaskUpdateStatusRoutingModule,
    CommercialUiModule,
    BaseThemeSharedModule,
    LocalizationModule,
    CoreModule,
    MatInputModule,
    NgbDropdownModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class TaskUpdateStatusModule { }
