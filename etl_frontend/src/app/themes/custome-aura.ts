// src/app/themes/custom-aura.ts

import { definePreset } from '@primeng/themes';
import Aura from '@primeng/themes/aura';

export const CustomAura = definePreset(Aura, {
  palette: {
    blue: {
      50: '#e6f5ff',
      100: '#ccebff',
      200: '#99d5ff',
      300: '#66c0ff',
      400: '#33abff',
      500: '#00aaff',
      600: '#0090e6',
      700: '#007ac0',
      800: '#006499',
      900: '#004d73',
      950: '#00374d'
    },
    slate: {
      50: '#f8fafc',
      100: '#f1f5f9',
      200: '#e2e8f0',
      300: '#cbd5e1',
      400: '#94a3b8',
      500: '#64748b',
      600: '#475569',
      700: '#334155',
      800: '#1e293b',
      900: '#0f172a',
      950: '#020617'
    }
  },

  semantic: {
    primary: {
      50: '{blue.50}',
      100: '{blue.100}',
      200: '{blue.200}',
      300: '{blue.300}',
      400: '{blue.400}',
      500: '{blue.500}',
      600: '{blue.600}',
      700: '{blue.700}',
      800: '{blue.800}',
      900: '{blue.900}',
      950: '{blue.950}'
    },
    colorScheme: {
      light: {
        primary: {
          color: '{blue.600}',
          contrastColor: '#ffffff',
          hoverColor: '{blue.700}',
          activeColor: '{blue.800}'
        },
        highlight: {
          background: '{surface.50}',
          focusBackground: '{surface.50}',
          color: '#ffffff',
          focusColor: '#ffffff'
        },
        surface: {
          0: '#ffffff',
          50: '{slate.50}',
          100: '{slate.100}',
          200: '{slate.200}',
          300: '{slate.300}',
          400: '{slate.400}',
          500: '{slate.500}',
          600: '{slate.600}',
          700: '{slate.700}',
          800: '{slate.800}',
          900: '{slate.900}',
          950: '{slate.950}'
        }
      },
      dark: {
        primary: {
          color: '{blue.500}',
          contrastColor: '#ffffff',
          hoverColor: '{blue.400}',
          activeColor: '{blue.300}'
        },
        highlight: {
          background: '{surface.50}',
          focusBackground: '{surface.50}',
          color: '#ffffff',
          focusColor: '#ffffff'
        },
        surface: {
          0: '{slate.950}',
          50: '{slate.900}',
          100: '{slate.800}',
          200: '{slate.700}',
          300: '{slate.600}',
          400: '{slate.500}',
          500: '{slate.400}',
          600: '{slate.300}',
          700: '{slate.200}',
          800: '{slate.100}',
          900: '{slate.50}',
          950: '#ffffff'
        }
      }
    }
  },

  components: {
    toolbar: {
      root: {
        background: '',
        color: '{surface.900}',
      },
      colorScheme: {
        dark: {
          root: {
            background: "",
            color: '{surface.950}',
          },
        },
      },
    },
  },
});
