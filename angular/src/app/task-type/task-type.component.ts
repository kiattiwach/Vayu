import {ListService, PagedResultDto} from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import {FormGroup, FormBuilder, Validators, FormControl} from '@angular/forms';
import {TaskTypeDto, taskTypeEnumOptions, TaskTypeService} from "@proxy/task-types";

@Component({
  selector: 'app-task-type',
  templateUrl: './task-type.component.html',
  styleUrls: ['./task-type.component.scss'],
  providers: [ListService]
})
export class TaskTypeComponent implements OnInit {
  taskType = {items: [], totalCount: 0 } as PagedResultDto<TaskTypeDto>;
  isModalOpen = false;
  taskTypes = taskTypeEnumOptions;
  form: FormGroup;

  constructor(public readonly list: ListService,
              private taskTypeService: TaskTypeService,
              private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const taskTypeStreamCreator = (query) => this.taskTypeService.getList(query);
    this.list.hookToQuery(taskTypeStreamCreator)?.subscribe((response) => this.taskType = response);

  }

  createTaskType() {
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      type: [null, Validators.required]
    });

  }

  save() {
    console.log(this.form.value)
    return;
    if (this.form.invalid) {
      return;
    }

    this.taskTypeService.create(this.form.value).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

}
