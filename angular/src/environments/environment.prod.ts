import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Vayu',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44375/',
    redirectUri: baseUrl,
    clientId: 'Vayu_App',
    responseType: 'code',
    scope: 'offline_access Vayu',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44375',
      rootNamespace: 'Vayu',
    },
  },
} as Environment;
