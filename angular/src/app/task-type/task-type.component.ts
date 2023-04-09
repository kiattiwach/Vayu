import {ABP, ListService, LocalizationService, PagedResultDto, PermissionService} from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import {FormGroup, FormBuilder, Validators, FormControl, Form} from '@angular/forms';
import {
  TaskTypeDto,
  taskCategoryOptions,
  TaskTypeService,
  TaskTypeCreateUpdateDto,
  TaskTypeFilterDto
} from "@proxy/task-types";
import { Confirmation } from '@abp/ng.theme.shared';
import {VayuServiceService} from "../services/vayu-service.service";

@Component({
  selector: 'app-task-type',
  templateUrl: './task-type.component.html',
  styleUrls: ['./task-type.component.scss'],
  providers: [ListService]
})
export class TaskTypeComponent implements OnInit {
  filters = {} as TaskTypeFilterDto;
  taskType = {items: [], totalCount: 0 } as PagedResultDto<TaskTypeDto>;
  selectTaskType = {} as TaskTypeDto;
  isModalOpen = false;
  categoryOptions = taskCategoryOptions;
  form: FormGroup;
  nameControl: FormControl;
  taskCategoryControl: FormControl;
  confirmationTitle: string;
  isDisplayActionMenu: boolean;
  snackBarMessage: string;

  constructor(public readonly list: ListService,
              public taskTypeService: TaskTypeService,
              private fb: FormBuilder,
              public vayuServiceService: VayuServiceService,
              private localizationService: LocalizationService,
              private permissionService: PermissionService) {
  }

  ngOnInit(): void {
    const streamCreator = (query) => this.taskTypeService.getList({...query,
      ...this.filters});
    this.list.hookToQuery(streamCreator)?.subscribe((response) => this.taskType = response);

    this.localizationService.get('::AreYouSureDeleteTaskType').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.TaskTypes.Edit') || this.permissionService.getGrantedPolicy('Vayu.TaskTypes.Delete')
  }

  create() {
    this.selectTaskType = {} as TaskTypeDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  edit(id: string){
    this.taskTypeService.get(id).subscribe(s => {
      this.selectTaskType = s;
      this.buildForm();
      this.isModalOpen  = true;
    });
  }

  delete(id: string, name: string) {
    this.vayuServiceService.displayConfirmDelete(this.confirmationTitle,name).subscribe((status) =>{
      if (status === Confirmation.Status.confirm) {
        let isError: boolean;
        this.taskTypeService.delete(id).subscribe(() => this.snackBarMessage = 'DeleteTaskTypeComplete',
          (error) => {
            this.snackBarMessage = 'DeleteTaskTypeFail';
            isError = true;
          },
          () => {
            this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
            this.list.get()
          });
      }
    });
  }

  buildForm() {
    this.nameControl =  new FormControl(this.selectTaskType.name || null,[Validators.required]);
    this.taskCategoryControl = new FormControl(this.selectTaskType.taskCategory != null ? this.selectTaskType.taskCategory : null,[Validators.required]);
    this.form = this.fb.group({
      name: this.nameControl,
      taskCategory: this.taskCategoryControl
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    let updateDto = <TaskTypeCreateUpdateDto>this.form.value;
    updateDto.id = this.selectTaskType.id;
    const request = this.selectTaskType.id
    ? this.taskTypeService.update(updateDto)
      :this.taskTypeService.create(this.form.value);

    let isError: boolean;
    request.subscribe(() => {
      this.snackBarMessage = this.selectTaskType.id ? 'EditTaskTypeCompleted' : 'AddTaskTypeCompleted';
    }, (error) => {
      this.snackBarMessage = `${this.selectTaskType.id ? 'EditTaskTypeFail' : 'AddTaskTypeFail'} ${error.message}`;
      isError = true;
    },() =>{
      this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  clearFilters() {
    this.filters = {} as TaskTypeFilterDto;

    this.list.get();
  }

}
