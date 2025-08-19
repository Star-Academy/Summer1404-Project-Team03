import { Routes } from '@angular/router';
import { NotFoundComponent } from './features/not-found/not-found.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
    canMatch: [], //TODO check this
    data: { role: "data_admin" }
  },
  {
    path: 'landing',
    loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent),
    canActivate: [] //TODO adding auth guard
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
