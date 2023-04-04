import {Component, OnInit, ViewChild} from '@angular/core';
import {ListService, LocalizationService, PagedResultDto, PermissionService} from "@abp/ng.core";
import {NgbDateAdapter, NgbDateNativeAdapter, NgbModalOptions} from "@ng-bootstrap/ng-bootstrap";
import {phonePattern} from '../shared/pattern';
import {
  Status,
  TaskCreateUpdateDto,
  TaskDto,
  TaskFilterDto,
  TaskService, TaskViewDto,
  TaskWithNavigatePropertiesDto
} from "@proxy/tasks";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {VayuServiceService} from "../services/vayu-service.service";
import {Confirmation} from "@abp/ng.theme.shared";
import {StaffDto, StaffService} from "@proxy/staffs";

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
  selectStaffControl: FormControl = new FormControl<StaffDto>(null);
  teamMembers: StaffDto[] = [];
  teamMemberSelected: StaffDto;
  staffOptions: StaffDto[] = [];
  snackBarMessage: string;
  taskViewDto: TaskViewDto;

    constructor(public readonly list: ListService,
                public taskService: TaskService,
                private fb: FormBuilder,
                private vayuServiceService: VayuServiceService,
                private localizationService: LocalizationService,
                private permissionService: PermissionService,
                private staffService: StaffService) { }

  ngOnInit(): void {
    this.createUpdateOptions = { size:'lg',  backdrop: 'static'};
    this.taskInfoOptions = { size:'lg'};
    const streamCreator = (query) => this.taskService.getList({...query,
      ...this.filters});
    this.list.hookToQuery(streamCreator)?.subscribe((response) => {
      this.tasks = response;
      setTimeout(() => {
        this.toggleGroup();
      }, 1500);
    });

    this.localizationService.get('::AreYouSureDeleteTask').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.Tasks.Edit') || this.permissionService.getGrantedPolicy('Vayu.Tasks.Delete')
    this.taskService.getTeamLookup([]).subscribe(x => this.staffOptions = x);

    this.selectStaffControl.valueChanges.subscribe((x) => {
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
    this.contactPersonControl = new FormControl(this.selectedTask.contactPerson || null);
    this.contactNumberControl = new FormControl(this.selectedTask.contactNumber || null, [Validators.pattern(this.phonePattern)]);
    this.teamMemberIdsControl = new FormControl(this.selectedTask.teamMemberIds || null, [Validators.required]);

    this.form = this.fb.group({
      taskTypeId: this.taskTypeIdControl,
      status: this.statusControl,
      description: this.descriptionControl,
      //createDateTime: this.createDateTimeControl,
      //completeDateTime: this.completeDateTimeControl,
      note: this.noteControl,
      leaderId: this.leaderIdControl,
      contactPerson: this.contactPersonControl,
      contactNumber: this.contactPersonControl,
      teamMemberIds: this.teamMemberIdsControl
    });
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

  //todo-remove
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

    if (!this.selectedTask.id) {
      this.statusControl.setValue(Status.Initial, {emitEvent: false});
      //this.createDateTimeControl.setValue(new Date(), {emitEvent: false});
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
        this.selectStaffControl.setValue(null,{emitEvent: false});
        let ids = this.teamMemberIdsControl.value ?? [];
        ids.push(id);
        this.teamMemberIdsControl.setValue(ids, {emitEvent: false});
      });
    }
  }
  teamChange(event: string[]) {
    this.teamMembers = this.teamMembers.filter(x => event?.some(id => id == x.id));
  }

  toggleExpandGroup(group) {
    this.table.groupHeader.toggleExpandGroup(group);
  }

  toggleGroup() {
    this.table.groupedRows.forEach(x => this.table.groupHeader.toggleExpandGroup(x));
  }
}
