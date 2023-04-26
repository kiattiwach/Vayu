import { Component } from '@angular/core';
import {HubService} from "./services/hub.service";

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
  `,
})
export class AppComponent {
  constructor(private hubService: HubService) {
    this.hubService.init();
  }
}
