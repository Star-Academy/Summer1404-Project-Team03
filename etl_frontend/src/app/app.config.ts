import {APP_INITIALIZER, ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {includeBearerTokenInterceptor, KeycloakService,} from 'keycloak-angular';


import {providePrimeNG} from 'primeng/config';
import {routes} from './app.routes';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {CustomAura} from './themes/custome-aura';
import {initializeKeycloak} from './configs/key-cloack.config';
import {CoreModule} from './core/core.module';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(CoreModule),
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    provideHttpClient(withInterceptors([includeBearerTokenInterceptor])),
    KeycloakService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      multi: true,
      deps: [KeycloakService],
    },
    providePrimeNG({
        theme: {
          preset: CustomAura,
          options: {
            prefix: 'p',
            darkModeSelector: 'dark-mode',
            cssLayer: false
          }
        },
        ripple: true
      }
    )
  ]
};
