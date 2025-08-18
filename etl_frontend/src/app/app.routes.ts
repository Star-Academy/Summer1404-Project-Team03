import {Routes} from '@angular/router';
import {canActivateAuthRole} from './guard/role-guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
    canMatch: [canActivateAuthRole],
    data: { role: "data_admin"}
  },
  {
    path: 'landing',
    loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent),
    // canActivate: [KeycloakAuthGuard] //TODO addin auth guard
  },
  // {
  //     path: '**',
  //     // redirectTo: //TODO add not found pages
  // }

];
