import { Component, OnInit } from '@angular/core';
import {ConfigStateService, ListService, LocalizationService, PagedResultDto, PermissionService} from "@abp/ng.core";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {VayuServiceService} from "../services/vayu-service.service";
import {
  StaffCreateUpdateDto, StaffDto,
  StaffFilterDto,
  StaffService,
  StaffWithNavigatePropertiesDto
} from "@proxy/staffs";
import {Confirmation} from "@abp/ng.theme.shared";
import {emailPattern, phonePattern} from "../shared/pattern";
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-staff',
  templateUrl: './staff.component.html',
  styleUrls: ['./staff.component.scss'],
  providers: [ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }]
})
export class StaffComponent implements OnInit {
  phonePattern = phonePattern;
  emailPattern = emailPattern;
  filters = {} as StaffFilterDto;
  staffs = {items: [], totalCount: 0 } as PagedResultDto<StaffWithNavigatePropertiesDto>;
  selectStaff = {} as StaffDto;
  isModalOpen = false;
  form: FormGroup;

  codeControl: FormControl;
  nameControl: FormControl;
  surnameControl: FormControl;
  codeNameControl: FormControl;
  emailControl: FormControl;
  addressControl: FormControl;
  radioCodeControl: FormControl;
  contactNumberControl: FormControl;
  contactPersonControl: FormControl;
  contactPersonNumberControl: FormControl;
  lineControl: FormControl;
  facebookControl: FormControl;
  roleControl: FormControl;
  startDateControl: FormControl;
  passAssessmentDateControl: FormControl;
  copyStartDateControl: FormControl;
  confirmationTitle: string;
  isDisplayActionMenu: boolean;
  snackBarMessage: string;
  isAdmin: boolean;

  constructor(public readonly list: ListService,
              public staffService: StaffService,
              private fb: FormBuilder,
              private vayuServiceService: VayuServiceService,
              private localizationService: LocalizationService,
              private permissionService: PermissionService,
              private config: ConfigStateService) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.staffService.getList({...query,
      ...this.filters});
    this.list.hookToQuery(streamCreator)?.subscribe((response) => this.staffs = response);

    this.localizationService.get('::AreYouSureDeleteStaff').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.Staffs.Edit') || this.permissionService.getGrantedPolicy('Vayu.Staffs.Delete');
    this.isAdmin = this.config.getDeep("currentUser.roles")?.some(x => x == 'admin');
  }

  create() {
    this.selectStaff = {} as StaffDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm() {
    this.codeControl=  new FormControl(this.selectStaff.code || null,[Validators.required]);
    this.nameControl=  new FormControl(this.selectStaff.name || null,[Validators.required]);
    this.surnameControl=  new FormControl(this.selectStaff.surname || null,[Validators.required]);
    this.codeNameControl=  new FormControl(this.selectStaff.codeName || null,[Validators.required]);
    this.emailControl=  new FormControl(this.selectStaff.email || null,[Validators.required, Validators.pattern(this.emailPattern)]);
    this.addressControl=  new FormControl(this.selectStaff.address || null,[]);
    this.radioCodeControl=  new FormControl(this.selectStaff.radioCode || null,[]);
    this.contactNumberControl=  new FormControl(this.selectStaff.contactNumber || null,[Validators.required, Validators.pattern(this.phonePattern)]);
    this.contactPersonControl=  new FormControl(this.selectStaff.contactPerson || null,[]);
    this.contactPersonNumberControl=  new FormControl(this.selectStaff.contactPersonNumber || null,[Validators.pattern(this.phonePattern)]);
    this.lineControl=  new FormControl(this.selectStaff.line || null,[]);
    this.facebookControl=  new FormControl(this.selectStaff.facebook || null,[]);
    this.roleControl=  new FormControl(this.selectStaff.roleId || null,[Validators.required]);
    this.startDateControl = new FormControl(this.selectStaff.startDate ? new Date(this.selectStaff.startDate) : null,[Validators.required]);
    this.passAssessmentDateControl = new FormControl(this.selectStaff.passAssessmentDate ? new Date(this.selectStaff.passAssessmentDate) : null, []);
    this.copyStartDateControl = new FormControl();

    this.form = this.fb.group({
      code: this.codeControl,
      name: this.nameControl,
      surname: this.surnameControl,
      codeName: this.codeNameControl,
      email: this.emailControl,
      address: this.addressControl,
      radioCode: this.radioCodeControl,
      contactNumber: this.contactNumberControl,
      contactPerson: this.contactPersonControl,
      contactPersonNumber: this.contactPersonNumberControl,
      line: this.lineControl,
      facebook: this.facebookControl,
      roleId: this.roleControl,
      startDate: this.startDateControl,
      passAssessmentDate: this.passAssessmentDateControl,
      copyStartDate: this.copyStartDateControl
    });
  }

  edit(id: string){
    this.staffService.get(id).subscribe(s => {
      this.selectStaff = s;
      this.buildForm();
      this.isModalOpen  = true;
    });
  }

  delete(id: string, name: string) {
    this.vayuServiceService.displayConfirmDelete(this.confirmationTitle,name).subscribe((status) =>{
      let isError: boolean;
      if (status === Confirmation.Status.confirm) {
        this.staffService.delete(id).subscribe(() => this.snackBarMessage = 'DeleteStaffComplete',
          (error) => {
            this.snackBarMessage = 'DeleteStaffFail';
            isError = true;
          },
          () => {
            this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
            this.list.get()
          });
      }
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    let updateDto = <StaffCreateUpdateDto>this.form.value;
    updateDto.id = this.selectStaff.id;
    const request = this.selectStaff.id
      ? this.staffService.update(updateDto)
      :this.staffService.create(this.form.value);

    let isError: boolean;
    request.subscribe(() => {
      this.snackBarMessage = this.selectStaff.id ? 'EditStaffCompleted' : 'AddStaffCompleted';
    }, (error) => {
      this.snackBarMessage = `${this.selectStaff.id ? 'EditStaffFail' : 'AddStaffFail'} ${error.message}`;
      isError = true;
    },() =>{
      this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  clearFilters() {
    this.filters = {} as StaffFilterDto;

    this.list.get();
  }

  copyStartDate(){
    if (this.startDateControl.value != null && this.copyStartDateControl.value === true)
      this.passAssessmentDateControl.setValue(this.startDateControl.value, {emitEvent:false});
    else
      this.passAssessmentDateControl.setValue(null,{emitEvent: false});
  }

  testSendMail() {
    const email = this.config.getDeep("currentUser.email");
    if (email){
      this.staffService.testSendMail(email)
        .subscribe(() => this.vayuServiceService.displaySnackBar('TestSendMailComplete'),
          (error => this.vayuServiceService.displaySnackBar('TestSendMailFail')));
    }
  }

  reSendRegistrationEmail(userId: string){

  }
}
