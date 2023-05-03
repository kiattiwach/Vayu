import { Component, OnInit } from '@angular/core';
import {AvailableStatusDto, Status, TaskService, TaskWithNavigatePropertiesDto} from "@proxy/tasks";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {ConfigStateService} from "@abp/ng.core";
import {StaffFilterDto, StaffService} from "@proxy/staffs";
import {LookupRequestDto} from "@proxy/shared";

@Component({
  selector: 'app-task-update-status',
  templateUrl: './task-update-status.component.html',
  styleUrls: ['./task-update-status.component.scss']
})
export class TaskUpdateStatusComponent implements OnInit {
  form: FormGroup;
  taskControl: FormControl<TaskWithNavigatePropertiesDto>;
  statusControl: FormControl<Status>;
  statusNoteControl: FormControl<string> = new FormControl<string>(null);
  userName: string;
  hasNote: boolean;

  constructor(private fb: FormBuilder,
              public taskService: TaskService,
              private config: ConfigStateService,
              private staffService: StaffService) { }

  ngOnInit(): void {
    this.staffService.getByEmail(this.config.getDeep("currentUser.userName"))
      .subscribe(s => this.userName = s.name);
    this.buildForm();
  }

  buildForm() {
    this.taskControl = new FormControl<TaskWithNavigatePropertiesDto>(null, [Validators.required]);
    this.statusControl = new FormControl<Status>(null, [Validators.required]);
    this.statusControl.valueChanges.subscribe((s) => {
      const statusInfo = this.taskControl.value.availableStatus?.find(x => x.status == s);
      this.hasNote = statusInfo?.requiredNote;
      this.statusNoteControl = statusInfo?.requiredNote == true ?
        new FormControl<string>(null, [Validators.required]) :
        new FormControl<string>(null)
    });

    this.taskControl.valueChanges.subscribe(s => {
      const statusInfo = s.availableStatus ? s.availableStatus[0]: null;
      this.statusControl.setValue(statusInfo?.status);
      this.hasNote = statusInfo?.requiredNote;
    });

    this.form = this.fb.group({
      task: this.taskControl,
      //status: this.statusControl,
      statusNote: this.statusNoteControl
    });
  }

  save() {

  }

}
