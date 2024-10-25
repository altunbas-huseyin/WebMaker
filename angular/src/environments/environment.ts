 import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44328/',
  redirectUri: baseUrl,
  clientId: 'WebMaker_App',
  responseType: 'code',
  scope: 'offline_access WebMaker',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'WebMaker',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44328',
      rootNamespace: 'WebMaker',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
