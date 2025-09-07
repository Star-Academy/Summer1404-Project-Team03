import {Routes} from "@angular/router";
import {DashboardComponent} from "./dashboard.component";

export const dashboardRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'workflows',
      },
      {
        path: 'workflows',
        loadChildren: () =>
          import('./components/manage-workflows/manage-workflows.module').then(
            (m) => m.ManageWorkflowsModule
          )
      },
      {
        path: 'files',
        loadChildren: () =>
          import('./components/manage-files/manage-files.module').then(
            (m) => m.ManageFilesModule
          ),
      },
    ],
  },
]
