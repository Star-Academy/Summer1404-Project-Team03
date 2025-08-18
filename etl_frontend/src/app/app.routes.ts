import {Routes} from '@angular/router';
import {AuthGuard} from './guard/auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
    canMatch: [AuthGuard], //TODO check this
    data: { role: "data_admin"}
  },
  {
    path: 'landing',
    loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent),
    canActivate: [] //TODO adding auth guard
  },
  // {
  //     path: '**',
  //     // redirectTo: //TODO add not found pages
  // }

];
