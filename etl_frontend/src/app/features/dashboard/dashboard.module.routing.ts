import {Routes} from "@angular/router";
import {DashboardComponent} from "./dashboard.component";

export const dashboardRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./components/home/home.module').then((m) => m.HomeModule)
      }
    ],
  },
]
