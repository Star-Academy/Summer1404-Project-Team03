import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
    },
    {
        path: 'dashboard',
        loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
        canMatch: [] //TODO addin auth guard
    },
    {
        path: '**',
        // redirectTo: //TODO add not found pages 
    }

];
