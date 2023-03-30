import { AccountConfigModule } from '@abp/ng.account/config';
import { CoreModule } from '@abp/ng.core';
import { registerLocale } from '@abp/ng.core/locale';
import { IdentityConfigModule } from '@abp/ng.identity/config';
import { SettingManagementConfigModule } from '@abp/ng.setting-management/config';
import { TenantManagementConfigModule } from '@abp/ng.tenant-management/config';
import { ThemeLeptonXModule } from '@abp/ng.theme.lepton-x';
import { SideMenuLayoutModule } from '@abp/ng.theme.lepton-x/layouts';
import {SUPPRESS_UNSAVED_CHANGES_WARNING, ThemeSharedModule} from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { storeLocaleData } from '@abp/ng.core/locale';
import {MatSnackBarModule} from "@angular/material/snack-bar";
import(
  /* webpackChunkName: "_locale-your-locale-js"*/
  /* webpackMode: "eager" */
  '@angular/common/locales/th'
  ).then((m) => storeLocaleData(m.default, 'th'));

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule.forRoot({
      environment,
      registerLocaleFn: registerLocale(),
    }),
    ThemeSharedModule.forRoot(),
    AccountConfigModule.forRoot(),
    IdentityConfigModule.forRoot(),
    TenantManagementConfigModule.forRoot(),
    SettingManagementConfigModule.forRoot(),
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
    MatSnackBarModule
  ],
  declarations: [AppComponent],
  providers: [APP_ROUTE_PROVIDER, {provide: SUPPRESS_UNSAVED_CHANGES_WARNING, useValue: true}],//todo - switch to true when found solution to overwrite general confirmation modal
  bootstrap: [AppComponent],
})
export class AppModule {}
