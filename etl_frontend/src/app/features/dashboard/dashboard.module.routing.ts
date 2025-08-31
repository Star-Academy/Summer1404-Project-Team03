import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard.component";

export const dashboardRoutes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        children: [
            {
                path: 'home',
                loadChildren: () => import('./components/home/home.module').then(m => m.HomeModule)
            },
            // main layout phase 3
        ]
    },
]
