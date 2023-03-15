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
        name:'::Menu:Setting',
        iconClass:'fas fa-cogs',
        order:2,
        layout: eLayoutType.application
      },
      {
        path: '/task-types',
        name: '::Menu:TaskTypes',
        iconClass: 'fas fa-list-alt',
        parentName: '::Menu:Setting',
        layout: eLayoutType.application,
        requiredPolicy: 'Vayu.TaskTypes'
      }
    ]);
  };
}
