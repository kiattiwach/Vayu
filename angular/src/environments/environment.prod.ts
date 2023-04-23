import { Environment } from '@abp/ng.core';

const baseUrl = 'https://vayu-service.azurewebsites.net';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Vayu',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://vayu-api.azurewebsites.net/',
    redirectUri: baseUrl,
    clientId: 'Vayu_App',
    responseType: 'code',
    scope: 'offline_access Vayu',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://vayu-api.azurewebsites.net',
      rootNamespace: 'Vayu',
    },
  },
} as Environment;
