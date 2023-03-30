import { Component, OnInit } from '@angular/core';
import {ListService, LocalizationService, PagedResultDto, PermissionService} from "@abp/ng.core";
import {RoleCreateUpdateDto, RoleDto, RoleFilterDto, RoleService} from "@proxy/roles";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {Confirmation} from "@abp/ng.theme.shared";
import {VayuServiceService} from "../services/vayu-service.service";

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss'],
  providers: [ListService]
})
export class RoleComponent implements OnInit {
  filters = {} as RoleFilterDto;
  role = {items: [], totalCount: 0 } as PagedResultDto<RoleDto>;
  selectRole = {} as RoleDto;
  isModalOpen = false;
  form: FormGroup;
  nameControl: FormControl;
  descriptionControl: FormControl;
  confirmationTitle: string;
  isDisplayActionMenu: boolean;
  snackBarMessage: string;

  constructor(public readonly list: ListService,
              private roleService: RoleService,
              private fb: FormBuilder,
              private vayuServiceService: VayuServiceService,
              private localizationService: LocalizationService,
              private permissionService: PermissionService) { }

  ngOnInit(): void {
    const streamCreator = (query) => this.roleService.getList({
      ...query,
      ...this.filters});
    this.list.hookToQuery(streamCreator)?.subscribe((response) => this.role = response);

    this.localizationService.get('::AreYouSureDeleteRole').subscribe(s => this.confirmationTitle = s);
    this.isDisplayActionMenu = this.permissionService.getGrantedPolicy('Vayu.Roles.Edit') || this.permissionService.getGrantedPolicy('Vayu.Roles.Delete');
  }

  create() {
    this.selectRole = {} as RoleDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  edit(id: string){
    this.roleService.get(id).subscribe(s => {
      this.selectRole = s;
      this.buildForm();
      this.isModalOpen  = true;
    });
  }

  delete(id: string, name: string) {
    this.vayuServiceService.displayConfirmDelete(this.confirmationTitle, name).subscribe((status) =>{
      if (status === Confirmation.Status.confirm) {
        let isError: boolean;
        this.roleService.delete(id).subscribe(() => this.snackBarMessage = 'DeleteRoleComplete',
          (error) => {
            this.snackBarMessage = 'DeleteRoleFail';
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
    this.nameControl =  new FormControl(this.selectRole.name || null,[Validators.required]);
    this.descriptionControl = new FormControl(this.selectRole.description || null,[]);
    this.form = this.fb.group({
      name: this.nameControl,
      description: this.descriptionControl
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    let updateDto = <RoleCreateUpdateDto>this.form.value;
    updateDto.id = this.selectRole.id;
    const request = this.selectRole.id
      ? this.roleService.update(updateDto)
      :this.roleService.create(this.form.value);

    let isError: boolean;
    request.subscribe(() => {
      this.snackBarMessage = this.selectRole.id ? 'EditRoleCompleted' : 'AddRoleCompleted';
    }, (error) => {
      this.snackBarMessage = `${this.selectRole.id ? 'EditRoleFail' : 'AddRoleFail'} ${error.message}`;
      isError = true;
    },() =>{
      this.vayuServiceService.displaySnackBar(this.snackBarMessage, isError);
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  clearFilters() {
    this.filters = {} as RoleFilterDto;

    this.list.get();
  }

}
