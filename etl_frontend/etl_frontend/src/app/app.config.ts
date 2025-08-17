import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { includeBearerTokenInterceptor, provideKeycloak, } from 'keycloak-angular';


import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([includeBearerTokenInterceptor])),
    provideKeycloak({
      config: {
        clientId: "my_frontend_app",
        realm: 'codestar',
        url: "http://192.168.25.178:8080",
      },
      initOptions: {
        onLoad: 'check-sso',
        checkLoginIframe: true,
      },
    }),
    providePrimeNG({
      theme: {
        preset: Aura,
      },
      ripple: true
    }
    )
  ]
};
