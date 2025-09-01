import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideStore } from '@ngrx/store'

import { CustomAura } from './themes/custome-aura';

import { CoreModule } from './core/core.module';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorHandlingInterceptor } from './shared/interceptors/error-handling/error-handling.interceptor';
import { credentialsInterceptor } from './shared/interceptors/credentials/credentials.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(CoreModule),
    provideAnimationsAsync(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    providePrimeNG({
      theme: {
        preset: CustomAura,
        options: {
          prefix: 'p',
          darkModeSelector: 'dark-mode',
          cssLayer: false,
        },
      },
      ripple: true,
    }),
    provideStore(),
    provideHttpClient(withInterceptors([errorHandlingInterceptor, credentialsInterceptor]))
  ],
};
