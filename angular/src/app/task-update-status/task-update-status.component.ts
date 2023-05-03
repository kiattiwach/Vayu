import { Component, OnInit } from '@angular/core';
import {AvailableStatusDto, TaskService, TaskWithNavigatePropertiesDto} from "@proxy/tasks";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-task-update-status',
  templateUrl: './task-update-status.component.html',
  styleUrls: ['./task-update-status.component.scss']
})
export class TaskUpdateStatusComponent implements OnInit {
  form: FormGroup;
  taskControl: FormControl;
  statusControl: FormControl;
  statusNoteControl: FormControl;

  constructor(private fb: FormBuilder,
              private taskService: TaskService) { }

  ngOnInit(): void {
    this.taskControl.valueChanges.subscribe(s => console.log(s));
  }

  buildForm() {
    this.taskControl = new FormControl<TaskWithNavigatePropertiesDto>(null, [Validators.required]);
    this.statusControl = new FormControl<AvailableStatusDto>(null, [Validators.required]);
    this.statusControl.valueChanges.subscribe((s: AvailableStatusDto) =>
      this.statusNoteControl =
        new FormControl<string>(null, [s.requiredNote == true ? Validators.required : null]));

    this.form = this.fb.group({
      task: this.taskControl,
      status: this.statusControl,
      statusNote: this.statusNoteControl
    });
  }

  save() {
    
  }

}
