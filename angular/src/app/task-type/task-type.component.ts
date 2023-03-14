import {ListService, PagedResultDto} from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import {TaskTypeDto, TaskTypeService} from "@proxy/task-types";

@Component({
  selector: 'app-task-type',
  templateUrl: './task-type.component.html',
  styleUrls: ['./task-type.component.scss'],
  providers: [ListService]
})
export class TaskTypeComponent implements OnInit {
  taskType = {items: [], totalCount: 0 } as PagedResultDto<TaskTypeDto>;
  isModalOpen = false;

  constructor(public readonly list: ListService,
              private taskTypeService: TaskTypeService)
  {
  }

  ngOnInit(): void {
    const taskTypeStreamCreator = (query) => this.taskTypeService.getList(query);
        this.list.hookToQuery(taskTypeStreamCreator)?.subscribe((response) => this.taskType = response);
  }

  createBook() {
    this.isModalOpen = true;
  }

}
