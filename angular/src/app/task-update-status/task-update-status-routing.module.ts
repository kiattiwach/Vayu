import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskUpdateStatusComponent } from './task-update-status.component';

const routes: Routes = [{ path: '', component: TaskUpdateStatusComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaskUpdateStatusRoutingModule { }
