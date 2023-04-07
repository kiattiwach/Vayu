import {Component, OnInit, ViewChild} from '@angular/core';
import {ConfigStateService, ListService, LocalizationService, PagedResultDto, PermissionService} from "@abp/ng.core";
import {NgbDateAdapter, NgbDateNativeAdapter, NgbModalOptions} from "@ng-bootstrap/ng-bootstrap";
import {getTimeSpan, phonePattern} from '../shared/pattern';
import {
  AvailableStatusDto,
  Status,
  TaskCreateUpdateDto,
  TaskDto,
  TaskFilterDto,
  TaskService, TaskStatusDto, TaskViewDto,
  TaskWithNavigatePropertiesDto
} from "@proxy/tasks";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {VayuServiceService} from "../services/vayu-service.service";
import {StaffDto, StaffService} from "@proxy/staffs";
import {Observable, of, switchMap} from "rxjs";
import { Guid } from "guid-typescript";
import {TaskTypeService} from "@proxy/task-types";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss'],
  providers: [ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }]
})
export class TaskComponent implements OnInit {
  @ViewChild('taskTable') table: any;
  @ViewChild('myGroupHeader') groupHeader: any;
  phonePattern = phonePattern;
  filters = {} as TaskFilterDto;
  tasks = {items: [], totalCount: 0 } as PagedResultDto<TaskWithNavigatePropertiesDto>;
  selectedTask = {} as TaskDto;
  isCreateUpdateModalOpen = false;
  isViewModalOpen = false;
  form: FormGroup;
  confirmationTitle: string;
  isDisplayActionMenu: boolean;
  createUpdateOptions: NgbModalOptions;
  taskInfoOptions: NgbModalOptions;
  taskTypeIdControl: FormControl;
  statusControl: FormControl;
  descriptionControl: FormControl;
  createDateTimeControl: FormControl;
  completeDateTimeControl: FormControl;
  noteControl: FormControl;
  leaderIdControl: FormControl;
  contactPersonControl: FormControl;
  contactNumberControl: FormControl;
  teamMemberIdsControl: FormControl;
  selectTeamMemberControl: FormControl = new FormControl<StaffDto>(null);
  teamMembers: StaffDto[] = [];
  snackBarMessage: string;
  taskViewDto: TaskViewDto;
  expandAll: boolean;
  updateStatusOption: NgbModalOptions;
  isUpdateStatusModalOpen = false;
  statusOptions: AvailableStatusDto[] = [];
  selectStatusControl: FormControl;
  selectStatusNoteControl: FormControl;
  formStatus: FormGroup;
  selectedTaskUpdateStatus: TaskWithNavigatePropertiesDto;
  requiredNote: boolean;
  selectedTeamMemberIds: string[] = [];
  teamMemberOptions: StaffDto[] = [];

    constructor(public readonly list: ListService,
                public taskService: TaskService,
                private fb: FormBuilder,
                private vayuServiceService: VayuServiceService,
                private localizationService: LocalizationService,
                private permissionService: PermissionService,
                private staffService: StaffService,
                private taskTypeService: TaskTypeService,
                private config: ConfigStateService) { }

  ngOnInit(): void {
    this.createUpdateOptions = { size:'lg',  backdrop: 'static'};
    this.taskInfoOptions = { size:'lg'};
    this.updateStatusOption = {size:'sm', backdrop: 'static'};
    const streamCreator = (query) => this.taskService.getList({...query,
      ...this.filters});
    this.list.hookToQuery(streamCreator)?.subscribe((response) => {
      this.tasks = response;
      setTimeout(() => {
        this.toggleGroup(true);
      }, 1000);
    });

    this.localizationService.get('::AreYouSureDeleteTask').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.Tasks.Edit') || this.permissionService.getGrantedPolicy('Vayu.Tasks.Delete')

    this.selectTeamMemberControl.valueChanges.subscribe((x) => {
      this.addTeamMember(x);
    });
  }

  create() {
    this.selectedTask = {} as TaskDto;
    this.buildForm();
    this.isCreateUpdateModalOpen = true;
  }

  buildForm() {
    this.taskTypeIdControl = new FormControl(this.selectedTask.taskTypeId || null, [Validators.required]);
    this.statusControl = new FormControl(this.selectedTask.status || null);
    this.descriptionControl = new FormControl(this.selectedTask.description || null, [Validators.required]);
    this.createDateTimeControl = new FormControl(this.selectedTask.createDateTime ? new Date(this.selectedTask.createDateTime) : null);
    this.completeDateTimeControl = new FormControl(this.selectedTask.completeDateTime ? new Date(this.selectedTask.completeDateTime) : null);
    this.noteControl = new FormControl(this.selectedTask.note || null);
    this.leaderIdControl = new FormControl(this.selectedTask.leaderId || null, [Validators.required]);
    this.leaderIdControl.valueChanges.subscribe((id) => {
      this.teamMembers = this.teamMembers.filter(x => x.id != id);
      this.taskService.getAvailableStaffs(this.getUnavailableStaffs()).subscribe(x => this.teamMemberOptions = x);
    })
    this.contactPersonControl = new FormControl(this.selectedTask.contactPerson || null);
    this.contactNumberControl = new FormControl(this.selectedTask.contactNumber || null, [Validators.pattern(this.phonePattern)]);
    this.teamMemberIdsControl = new FormControl(this.selectedTask.teamMemberIds || null, [Validators.required]);

    if (this.selectedTask.id) {
      this.selectedTeamMemberIds = this.selectedTask.teamMemberIds;
      this.taskService.getStaffMany(this.selectedTask.teamMemberIds)
        .subscribe(x => this.teamMembers = x);
    } else {
      this.selectedTeamMemberIds = [];
    }

    this.taskService.getAvailableStaffs(this.getUnavailableStaffs()).subscribe(x => this.teamMemberOptions = x);

    this.form = this.fb.group({
      taskTypeId: this.taskTypeIdControl,
      status: this.statusControl,
      description: this.descriptionControl,
      note: this.noteControl,
      leaderId: this.leaderIdControl,
      contactPerson: this.contactPersonControl,
      contactNumber: this.contactNumberControl,
      teamMemberIds: this.teamMemberIdsControl
    });
  }

  private getUnavailableStaffs(): string[] {
    let result: string[] = [];
    if (this.leaderIdControl.value)
      result.push(this.leaderIdControl.value);

    if (this.selectedTeamMemberIds?.length > 0)
      result.push(...this.selectedTeamMemberIds);

    if (result.length == 0)
      result.push(Guid.EMPTY);
    return result;
  }

  edit(id: string){
    this.taskService.get(id).subscribe(s => {
      this.selectedTask = s;
      this.buildForm();
      this.isCreateUpdateModalOpen  = true;
    });
  }

  view(id: string){
    this.taskService.getTaskInfo(id).subscribe((s) => {
      this.isViewModalOpen = true;
      this.taskViewDto = s;
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    if (!this.selectedTask.id) {
      this.statusControl.setValue(Status.Initial, {emitEvent: false});
    } else {
      this.statusControl.setValue(this.selectedTask.status, {emitEvent: false});
    }

    let dto = <TaskCreateUpdateDto>this.form.value;
    dto.id = this.selectedTask.id;
    const request = this.selectedTask.id
      ? this.taskService.update(dto)
      :this.taskService.create(this.form.value);

    let isError: boolean;
    request.subscribe(() => {
      this.snackBarMessage = this.selectedTask.id ? 'EditTaskCompleted' : 'AddTaskCompleted';
    }, (error) => {
      this.snackBarMessage = `${this.selectedTask.id ? 'EditTaskFail' : 'AddTaskFail'} ${error.message}`;
      isError = true;
    },() =>{
      this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
      this.isCreateUpdateModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  clearFilters() {
    this.filters = {} as TaskFilterDto;

    this.list.get();
  }

  addTeamMember(id: string){
    if (id) {
      this.staffService.get(id).subscribe((x) => {
        this.teamMembers.push(x);
        this.selectTeamMemberControl.setValue(null,{emitEvent: false});
        this.selectedTeamMemberIds = <string[]>this.teamMemberIdsControl.value ?? [];
        this.selectedTeamMemberIds.push(id);
        this.teamMemberIdsControl.setValue(this.selectedTeamMemberIds, {emitEvent: false});

        this.taskService.getAvailableStaffs(this.getUnavailableStaffs()).subscribe(x => this.teamMemberOptions = x);
      });
    }
  }

  teamChange(event: string[]) {
    this.teamMembers = this.teamMembers.filter(x => event?.some(id => id == x.id));
    this.selectedTeamMemberIds = this.teamMembers.map(x => x.id);
    this.taskService.getAvailableStaffs(this.getUnavailableStaffs()).subscribe(x => this.teamMemberOptions = x);
  }

  toggleExpandGroup(group) {
    this.table.groupHeader.toggleExpandGroup(group);
  }

  toggleGroup(toggle: boolean) {
    if (toggle == this.expandAll) return;
    this.table.groupedRows.forEach(x => this.table.groupHeader.toggleExpandGroup(x));
    this.expandAll = toggle;
  }

  getLastStatusDurationText(taskStatuses: TaskStatusDto[]): Observable<string> {
      const taskStatus = taskStatuses[taskStatuses.length - 1];
      if (taskStatus == null) return of('');

      return this.getDurationText(taskStatus.creationTime.toString(),null);
  }

  getDurationText(start: string, end: string): Observable<string> {
    const startDate = new Date(start);
    const completeDate = end ? new Date(end) : new Date();
    const timeSpan = getTimeSpan(startDate, completeDate);
    if (timeSpan.day == 0 || !timeSpan.day) {
      return this.localizationService.get('::DurationHM', ...[timeSpan.hour.toString(), timeSpan.minute.toString()]);
    } else {
      return this.localizationService.get('::DurationDH', ...[timeSpan.day.toString(), timeSpan.hour.toString(), timeSpan.minute.toString()]);
    }
  }

  canUpdateStatus(userId: string, creatorId: string) : boolean {
    const currentUserId = this.config.getDeep("currentUser.id");
    return this.permissionService.getGrantedPolicy('Vayu.Tasks.UpdateStatus') || currentUserId == userId
      || currentUserId == creatorId;
  }

  openUpdateStatus(task: TaskWithNavigatePropertiesDto) {
    this.selectedTaskUpdateStatus = task;
    this.statusOptions = task.availableStatus;

    this.selectStatusControl = new FormControl(this.statusOptions[0].status, [Validators.required]);
    this.selectStatusControl.valueChanges.subscribe((x) => {
      const availableStatus = this.selectedTaskUpdateStatus.availableStatus.find(f => f.status == x);
      this.requiredNote = availableStatus.requiredNote;
      this.selectStatusNoteControl.setValue(null,{emitEvent: false});
      if (this.requiredNote == true )
        this.selectStatusNoteControl.setValidators([Validators.required]);
      else
        this.selectStatusNoteControl.clearValidators();

      this.selectStatusNoteControl.updateValueAndValidity();
    });

    this.requiredNote = this.statusOptions.find(x => x.status == this.selectStatusControl.value).requiredNote;
    this.selectStatusNoteControl = this.requiredNote == true ? new FormControl(null,[Validators.required]) : new FormControl();
    this.formStatus = this.fb.group({
      status: this.selectStatusControl,
      note: this.selectStatusNoteControl
    });

    this.isUpdateStatusModalOpen = true;
  }

  getUpdateStatusHeader(): Observable<string> {
    return this.localizationService.get('::Enum:Status:' + this.selectedTaskUpdateStatus.task.status)
      .pipe(switchMap((x) => {
        return this.localizationService.get('::UpdateStatusFrom', ...[x])
      }));
  }

  updateStatus(){
    if (this.formStatus.invalid) {
      return;
    }

    let input = <TaskStatusDto>this.formStatus.value;
    input.taskId = this.selectedTaskUpdateStatus.task.id;
    let isError: boolean;
    this.taskService.updateTaskStatus(input).subscribe(() => {
      this.snackBarMessage = 'UpdateStatusComplete';
    }, (error) => {
      this.snackBarMessage = `UpdateStatusFail ${error.message}`;
      isError = true;
    },() =>{
      console.log('complete')
      this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
      this.isUpdateStatusModalOpen = false;
      this.formStatus.reset();
      this.list.get();
    });
  }

  getColor(color: any) {
    return '5px solid #'+color;
  }
}
