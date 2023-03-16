import {ListService, LocalizationService, PagedResultDto, PermissionService} from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import {FormGroup, FormBuilder, Validators, FormControl, Form} from '@angular/forms';
import {TaskTypeDto, taskCategoryOptions, TaskTypeService, TaskTypeCreateUpdateDto} from "@proxy/task-types";
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-task-type',
  templateUrl: './task-type.component.html',
  styleUrls: ['./task-type.component.scss'],
  providers: [ListService]
})
export class TaskTypeComponent implements OnInit {
  taskType = {items: [], totalCount: 0 } as PagedResultDto<TaskTypeDto>;
  selectTaskType = {} as TaskTypeDto;
  isModalOpen = false;
  categoryOptions = taskCategoryOptions;
  form: FormGroup;
  nameControl: FormControl;
  taskCategoryControl: FormControl;
  yesText: string;
  confirmationTitle: string;
  isDisplayActionMenu: boolean;

  constructor(public readonly list: ListService,
              private taskTypeService: TaskTypeService,
              private fb: FormBuilder,
              private confirmation: ConfirmationService,
              private localizationService: LocalizationService,
              private permissionService: PermissionService) {
  }

  ngOnInit(): void {
    const taskTypeStreamCreator = (query) => this.taskTypeService.getList(query);
    this.list.hookToQuery(taskTypeStreamCreator)?.subscribe((response) => this.taskType = response);
    this.localizationService.get('::Yes').subscribe(s => this.yesText = s);
    this.localizationService.get('::AreYouSureDeleteTaskType').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.TaskTypes.Edit') || this.permissionService.getGrantedPolicy('Vayu.TaskTypes.Delete')
  }

  createTaskType() {
    this.selectTaskType = {} as TaskTypeDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editTaskType(id: string){
    this.taskTypeService.get(id).subscribe(s => {
      this.selectTaskType = s;
      this.buildForm();
      this.isModalOpen  = true;
    });
  }

  delete(id: string, name: string) {
    const options: Partial<Confirmation.Options> = {
      hideCancelBtn: false,
      hideYesBtn: false,
      dismissible: false,
      cancelText: "::No",
      yesText: '<i class="fa fa-trash-alt me-1 text-danger"></i>'+this.yesText,
      icon: 'fa fa-trash-alt'
      /*messageLocalizationParams: ["Demo"],
      titleLocalizationParams: [],*/
      // You can customize icon
      // icon: 'fa fa-exclamation-triangle', // or
      // iconTemplate : '<img src="custom-image-path.jpg" alt=""/>'
    };

    this.confirmation.warn('::AreYouSureToDelete', this.confirmationTitle +' '+ name, options).subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.taskTypeService.delete(id).subscribe(() => this.list.get());
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

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

}
