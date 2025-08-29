  import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
  import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
  import { providePrimeNG } from 'primeng/config';
  import { provideRouter } from '@angular/router';
  import { routes } from './app.routes';
  import { provideStore } from '@ngrx/store'

  import { CustomAura } from './themes/custome-aura';

  import { CoreModule } from './core/core.module';
  import {HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
  import {CredentialsInterceptor} from './shared/interceptors/crendential.interceptor'

  export const appConfig: ApplicationConfig = {
    providers: [
      importProvidersFrom(CoreModule),
      provideAnimationsAsync(),
      provideZoneChangeDetection({eventCoalescing: true}),
      provideRouter(routes),
      provideAnimationsAsync(),
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
      {
        provide: HTTP_INTERCEPTORS,
        useClass: CredentialsInterceptor,
        multi: true,
      },
      provideHttpClient(withInterceptorsFromDi())
    ],
  };
