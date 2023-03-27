import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/tasks',
        name: '::Menu:Tasks',
        iconClass: 'fas fa-tasks',
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.Tasks',
        order:1
      },
      {
        name:'::Menu:Setting',
        iconClass:'fas fa-cogs',
        order:3,
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.Settings'
      },
      {
        path: '/task-types',
        name: '::Menu:TaskTypes',
        iconClass: 'fas fa-list-alt',
        parentName: '::Menu:Setting',
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.TaskTypes',
        order:1
      },
      {
        name: '::Menu:StaffRole',
        iconClass: 'fas fa-user-cog',
        parentName: '::Menu:Setting',
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.Settings',
        order:2
      },
      {
        path: '/roles',
        name: '::Menu:Roles',
        iconClass: 'fas fa-user-tag',
        parentName: '::Menu:StaffRole',
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.Roles',
        order: 1
      },
      {
        path: '/staffs',
        name: '::Menu:Staffs',
        iconClass: 'fas fa-users',
        parentName: '::Menu:StaffRole',
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.Staffs',
        order: 2
      }
    ]);
  };
}
