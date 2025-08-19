// src/app/themes/custom-aura.ts

import { definePreset } from '@primeng/themes';
import Aura from '@primeng/themes/aura';

export const CustomAura = definePreset(Aura, {
    palette: {
        cyan: {
            50: '#f0f9fa',
            100: '#e0f2f7',
            200: '#bae6f2',
            300: '#7dd3e7',
            400: '#38bdf8',
            500: '#06b6d4',
            600: '#0891b2',
            700: '#0e7490',
            800: '#155e75',
            900: '#164e63',
            950: '#083344'
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
            50: '{cyan.50}',
            100: '{cyan.100}',
            200: '{cyan.200}',
            300: '{cyan.300}',
            400: '{cyan.400}',
            500: '{cyan.500}',
            600: '{cyan.600}',
            700: '{cyan.700}',
            800: '{cyan.800}',
            900: '{cyan.900}',
            950: '{cyan.950}'
        },
        colorScheme: {
            light: {
                primary: {
                    color: '{cyan.500}',
                    contrastColor: '#ffffff',
                    hoverColor: '{cyan.600}',
                    activeColor: '{cyan.700}'
                },
                highlight: {
                    background: '{cyan.500}',
                    focusBackground: '{cyan.400}',
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
                    color: '{cyan.400}',
                    contrastColor: '#ffffff',
                    hoverColor: '{cyan.300}',
                    activeColor: '{cyan.200}'
                },
                highlight: {
                    background: '{cyan.400}',
                    focusBackground: '{cyan.300}',
                    color: '#ffffff',
                    focusColor: '{cyan.950}'
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
    }
});