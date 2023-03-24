import { Component, OnInit } from '@angular/core';
import {ListService, LocalizationService, PagedResultDto, PermissionService} from "@abp/ng.core";
import {NgbDateAdapter, NgbDateNativeAdapter} from "@ng-bootstrap/ng-bootstrap";
import { phonePattern } from '../shared/pattern';
import {TaskCreateUpdateDto, TaskDto, TaskFilterDto, TaskService, TaskWithNavigatePropertiesDto} from "@proxy/tasks";
import {FormBuilder, FormGroup} from "@angular/forms";
import {VayuServiceService} from "../services/vayu-service.service";
import {Confirmation} from "@abp/ng.theme.shared";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss'],
  providers: [ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }]
})
export class TaskComponent implements OnInit {
  phonePattern = phonePattern;
  filters = {} as TaskFilterDto;
  tasks = {items: [], totalCount: 0 } as PagedResultDto<TaskWithNavigatePropertiesDto>;
  selectedTask = {} as TaskDto;
  isModalOpen = false;
  form: FormGroup;
  confirmationTitle: string;
  isDisplayActionMenu: boolean;

    constructor(public readonly list: ListService,
                public taskService: TaskService,
                private fb: FormBuilder,
                private vayuServiceService: VayuServiceService,
                private localizationService: LocalizationService,
                private permissionService: PermissionService) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.taskService.getList({...query,
      ...this.filters});
    this.list.hookToQuery(streamCreator)?.subscribe((response) => this.tasks = response);

    this.localizationService.get('::AreYouSureDeleteTask').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.Tasks.Edit') || this.permissionService.getGrantedPolicy('Vayu.Tasks.Delete')
  }

  create() {
    this.selectedTask = {} as TaskDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm() {

  }

  edit(id: string){
    this.taskService.get(id).subscribe(s => {
      this.selectedTask = s;
      this.buildForm();
      this.isModalOpen  = true;
    });
  }

  delete(id: string, name: string) {
    this.vayuServiceService.displayConfirmDelete(this.confirmationTitle,name).subscribe((status) =>{
      if (status === Confirmation.Status.confirm) {
        this.taskService.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    let updateDto = <TaskCreateUpdateDto>this.form.value;
    updateDto.id = this.selectedTask.id;
    const request = this.selectedTask.id
      ? this.taskService.update(updateDto)
      :this.taskService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  clearFilters() {
    this.filters = {} as TaskFilterDto;

    this.list.get();
  }


}
