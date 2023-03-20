import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoleRoutingModule } from './role-routing.module';
import { RoleComponent } from './role.component';
import {AdvancedEntityFiltersModule, CommercialUiModule} from "@volo/abp.commercial.ng.ui";
import {CoreModule, LocalizationModule} from "@abp/ng.core";
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {BaseThemeSharedModule} from "@abp/ng.theme.shared";
import {ReactiveFormsModule} from "@angular/forms";
import {NgbDropdownModule} from "@ng-bootstrap/ng-bootstrap";


@NgModule({
  declarations: [
    RoleComponent
  ],
  imports: [
    CommonModule,
    RoleRoutingModule,
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
export class RoleModule { }
