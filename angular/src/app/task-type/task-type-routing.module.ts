import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskTypeComponent } from './task-type.component';
import {AuthGuard, PermissionGuard} from "@abp/ng.core";

const routes: Routes = [{ path: '', component: TaskTypeComponent, canActivate: [AuthGuard, PermissionGuard] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaskTypeRoutingModule { }
