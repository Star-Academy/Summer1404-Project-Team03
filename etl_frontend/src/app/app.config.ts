import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { provideRouter } from '@angular/router';
import { providePrimeNG } from 'primeng/config';
import { routes } from './app.routes';
import { CustomAura } from './themes/custome-aura';
import { CoreModule } from './core/core.module';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(CoreModule),
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
  ],
};
