import { Injectable } from '@angular/core';
import {Confirmation, ConfirmationService} from "@abp/ng.theme.shared";
import {Observable} from "rxjs";
import {LocalizationService} from "@abp/ng.core";
import {MatSnackBar} from "@angular/material/snack-bar";
import {snackBarConfig, snackBarErrorConfig} from "../shared/snack-bar-config";

@Injectable({
  providedIn: 'root'
})
export class VayuServiceService {
  private confirmationYesText: string;
  private confirmationNoText: string;
  constructor( private confirmation: ConfirmationService,
               private localizationService: LocalizationService,
               private snackBar: MatSnackBar) {
    this.localizationService.get('::Yes').subscribe(s => this.confirmationYesText = s);
    this.localizationService.get('::No').subscribe(s => this.confirmationNoText = s);
  }

  displayConfirmDelete(title: string, name: string) : Observable<Confirmation.Status> {
    const options: Partial<Confirmation.Options> = {
      hideCancelBtn: false,
      hideYesBtn: false,
      dismissible: false,
      cancelText: this.confirmationNoText,
      yesText: '<i class="fa fa-trash-alt me-1 text-danger"></i>'+ this.confirmationYesText,
      icon: 'fa fa-trash-alt'
      /*messageLocalizationParams: ["Demo"],
      titleLocalizationParams: [],*/
      // You can customize icon
      // icon: 'fa fa-exclamation-triangle', // or
      // iconTemplate : '<img src="custom-image-path.jpg" alt=""/>'
    };
    return this.confirmation.warn('::AreYouSureToDelete', title +' '+ name, options)
  }

  displaySnackBar(message: string, isError: boolean = false) {
    let action: string;
    this.localizationService.get(`::${message}`).subscribe(s => message = s);
    this.localizationService.get('::Close').subscribe(s => action = s);
    this.snackBar.open(message, action, isError ? snackBarErrorConfig : snackBarConfig);
  }

  removeEmptyGuidProp(object: any, propName: string){
    if (object.hasOwnProperty(propName) && object[propName] == '') {
     delete object[propName];
   }
  }
}
