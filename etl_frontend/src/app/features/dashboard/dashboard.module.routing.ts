import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard.component";


export const dashboardRoutes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    {
        path: 'profile', //TODO change it to me or account ?
        loadChildren: ()=> import('./components/profile/profile.module').then(m => m.ProfileModule)
    }
]