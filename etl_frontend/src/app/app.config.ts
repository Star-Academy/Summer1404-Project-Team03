import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';
import {providePrimeNG} from 'primeng/config';
import {routes} from './app.routes';
import {CustomAura} from './themes/custome-aura';
import {CoreModule} from './core/core.module';
import {provideStore} from '@ngrx/store';
import {provideHttpClient} from '@angular/common/http';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(CoreModule),
    provideAnimationsAsync(),
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    providePrimeNG({
      theme: {
        preset: CustomAura,
        options: {
          prefix: 'p',
          // darkModeSelector: 'dark-mode',
          cssLayer: false,
        },
      },
      ripple: true,
    }),
    provideStore(),
    provideHttpClient(  )
  ],
};
