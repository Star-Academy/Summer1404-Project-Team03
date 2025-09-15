import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard.component";

export const dashboardRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'workflow'
      },
      {
        path: 'workflow',
        loadChildren: () => import('./components/workflow-workspace/workflow-workspace.module').then(m => m.WorkflowEditorModule)
      },
      {
        path: '',
        loadChildren: () => import('./components/home/home.module').then((m) => m.HomeModule)
      },
    ],
  },
]
