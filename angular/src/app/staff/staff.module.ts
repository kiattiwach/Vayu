import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffRoutingModule } from './staff-routing.module';
import { StaffComponent } from './staff.component';
import {AdvancedEntityFiltersModule, CommercialUiModule} from "@volo/abp.commercial.ng.ui";
import {CoreModule, LocalizationModule} from "@abp/ng.core";
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {BaseThemeSharedModule} from "@abp/ng.theme.shared";
import {ReactiveFormsModule} from "@angular/forms";
import {NgbDropdownModule} from "@ng-bootstrap/ng-bootstrap";


@NgModule({
  declarations: [
    StaffComponent
  ],
  imports: [
    CommonModule,
    StaffRoutingModule,
    LocalizationModule,
    NgxDatatableModule,
    BaseThemeSharedModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    CoreModule,
    CommercialUiModule,
    AdvancedEntityFiltersModule
  ]
})
export class StaffModule { }
