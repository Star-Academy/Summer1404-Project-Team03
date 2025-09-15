import { Routes } from '@angular/router';
import { NotFoundComponent } from './features/not-found/not-found.component';
import { sysAdminGuard } from './shared/guards/sys-admin.guard';
import { authGuard } from './shared/guards/auth/auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
    // redirectTo: 'landing'
  },
  {
    path: 'dashboard',
    canMatch: [authGuard],
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
  },
  {
    path: 'landing',
    loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent),
  },
  {
    path: 'send-token-code',
    loadComponent: () => import('./features/send-token-code/send-token-code.component').then(m => m.SendTokenCodeComponent),
  },
  {
    path: 'profile',
    loadChildren: () => import('./features/profile/profile.module').then(m => m.ProfileModule),
  },
  {
    path: '**',
    redirectTo: 'not-found'
  },
  {
    path: 'not-found',
    component: NotFoundComponent
  }
];
